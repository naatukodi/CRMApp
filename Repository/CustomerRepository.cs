using Microsoft.Azure.Cosmos;
using CRMApp.Models;

namespace CRMApp.Repository;
public class CustomerRepository
{
    private readonly Container _container;

    public CustomerRepository(CosmosClient cosmosClient, IConfiguration configuration)
    {
        _container = cosmosClient.GetContainer(configuration["CosmosDb:DatabaseName"], configuration["CosmosDb:CustomerContainerName"]);
    }

    public async Task AddCustomerAsync(Customer customer)
    {
        await _container.CreateItemAsync(customer, new PartitionKey(customer.id));
    }

    public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
    {
        var query = "SELECT * FROM c";
        var queryDefinition = new QueryDefinition(query);
        var resultSet = _container.GetItemQueryIterator<Customer>(queryDefinition);

        var results = new List<Customer>();
        while (resultSet.HasMoreResults)
        {
            var response = await resultSet.ReadNextAsync();
            results.AddRange(response);
        }

        return results;
    }
}
