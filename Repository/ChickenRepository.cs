using CRMApp.Services;
using Microsoft.Azure.Cosmos;
using CRMApp.Models;

namespace CRMApp.Repository;

public class ChickenRepository : IChickenRepository
{
    private readonly ICosmosDbService _cosmosDbService;

    public ChickenRepository(ICosmosDbService cosmosDbService)
    {
        _cosmosDbService = cosmosDbService;
    }

    public async Task<List<Chicken>> GetChickensByUserIdAsync(string customerId)
    {
        var queryDefinition = new QueryDefinition("SELECT * FROM c WHERE c.customerId = @customerId")
                                .WithParameter("@customerId", customerId);

        return await _cosmosDbService.GetItemsAsync<Chicken>(queryDefinition);
    }
}
