using CRMApp.Services;
using Microsoft.Azure.Cosmos;
using CRMApp.Models;

namespace CRMApp.Repository;

public class BatchService : IBatchService
{
    private readonly ICosmosDbService _cosmosDbService;

    public BatchService(ICosmosDbService cosmosDbService)
    {
        _cosmosDbService = cosmosDbService;
    }

    public async Task<Batch> CreateBatchAsync(Batch batch)
    {
        batch.id = Guid.NewGuid().ToString();
        batch.DateCreated = DateTime.UtcNow;

        await _cosmosDbService.AddItemAsync(batch, batch.customerId);
        return batch;
    }

    public async Task<Batch> GetBatchByIdAsync(string type, string batchId)
    {
        var queryDefinition = new QueryDefinition("SELECT * FROM c WHERE c.batchId = @batchId and c.Type = @type")
                                .WithParameter("@batchId", batchId)
                                .WithParameter("@type", type);


        var results = await _cosmosDbService.GetItemsAsync<Batch>(queryDefinition);
        return results.FirstOrDefault(); // Expecting only one batch to match
    }

    public async Task<List<Chicken>> GetChickensByBatchIdAsync(string customerId, string batchId)
    {
        var queryDefinition = new QueryDefinition("SELECT * FROM c WHERE c.type = @type AND c.customerId = @customerId AND c.batchId = @batchId")
                                .WithParameter("@type", "chicken")
                                .WithParameter("@customerId", customerId)
                                .WithParameter("@batchId", batchId);

        return await _cosmosDbService.GetItemsAsync<Chicken>(queryDefinition);
    }

    public async Task<List<Batch>> GetBatchesByUserIdAsync(string customerId)
    {
        var queryDefinition = new QueryDefinition("SELECT * FROM c WHERE c.type = @type AND c.customerId = @customerId")
                                .WithParameter("@type", "batch")
                                .WithParameter("@customerId", customerId);

        return await _cosmosDbService.GetItemsAsync<Batch>(queryDefinition);
    }

}
