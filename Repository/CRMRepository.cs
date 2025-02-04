using Microsoft.Azure.Cosmos;
using CRMApp.Models;

namespace CRMApp.Repository;

public class CRMRepository
{
    private readonly Container _container;

    public CRMRepository(CosmosClient cosmosClient, IConfiguration configuration)
    {
        _container = cosmosClient.GetContainer(configuration["CosmosDb:DatabaseName"], configuration["CosmosDb:ContainerName"]);
    }

    public async Task AddDocumentAsync<T>(T document)
    {
        await _container.CreateItemAsync(document, new PartitionKey(GetPartitionKey(document)));
    }

    public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
    {
        var query = new QueryDefinition("SELECT * FROM c WHERE c.type = 'customer'");
        var resultSet = _container.GetItemQueryIterator<Customer>(query);

        var results = new List<Customer>();
        while (resultSet.HasMoreResults)
        {
            var response = await resultSet.ReadNextAsync();
            results.AddRange(response);
        }

        return results;
    }

    public async Task<IEnumerable<Feedback>> GetFeedbackByCustomerAsync(string customerId)
    {
        var query = new QueryDefinition("SELECT * FROM c WHERE c.type = 'feedback' AND c.customerId = @customerId")
            .WithParameter("@customerId", customerId);
        var resultSet = _container.GetItemQueryIterator<Feedback>(query);

        var results = new List<Feedback>();
        while (resultSet.HasMoreResults)
        {
            var response = await resultSet.ReadNextAsync();
            results.AddRange(response);
        }

        return results;
    }

    private string GetPartitionKey<T>(T document)
    {
        if (document is Customer customer)
            return customer.customerId;
        if (document is Feedback feedback)
            return feedback.customerId;

        throw new ArgumentException("Unsupported document type");
    }
}
