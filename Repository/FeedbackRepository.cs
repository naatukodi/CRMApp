using Microsoft.Azure.Cosmos;
using CRMApp.Models;

namespace CRMApp.Repository;
public class FeedbackRepository
{
    private readonly Container _container;

    public FeedbackRepository(CosmosClient cosmosClient, IConfiguration configuration)
    {
        _container = cosmosClient.GetContainer(configuration["CosmosDb:DatabaseName"], configuration["CosmosDb:FeedbackContainerName"]);
    }

    public async Task AddFeedbackAsync(Feedback feedback)
    {
        await _container.CreateItemAsync(feedback, new PartitionKey(feedback.customerId));
    }

    public async Task<IEnumerable<Feedback>> GetFeedbackByCustomerAsync(string customerId)
    {
        var query = new QueryDefinition("SELECT * FROM c WHERE c.customerId = @customerId")
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
}
