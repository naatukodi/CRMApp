using Microsoft.Azure.Cosmos;

namespace CRMApp.Repository
{
    public interface ICosmosDbService
    {
        Task AddItemAsync<T>(T item, string partitionKey);
        Task<T> GetItemAsync<T>(string id, string partitionKey);
        Task<List<T>> GetItemsAsync<T>(QueryDefinition queryDefinition);
        Task UpdateItemAsync<T>(string id, T item, string partitionKey);
        Task DeleteItemAsync(string id, string partitionKey);
    }
}
