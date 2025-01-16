using CRMApp.Models;
using Microsoft.Azure.Cosmos;
using System.Threading.Tasks;

namespace CRMApp.Repository
{
    public class ChickenFarmingRepository
    {
        private readonly Container _container;

        public ChickenFarmingRepository(CosmosClient cosmosClient, IConfiguration configuration)
        {
            var databaseName = configuration["CosmosDb:DatabaseName"];
            var containerName = configuration["CosmosDb:ContainerName"];
            _container = cosmosClient.GetContainer(databaseName, containerName);
        }

        public async Task AddChickenFarmingAsync(ChickenFarming farming)
        {
            await _container.CreateItemAsync(farming, new PartitionKey(farming.CustomerId));
        }

        public async Task AddFarmerFeedbackAsync(FarmerFeedback feedback)
        {
            await _container.CreateItemAsync(feedback, new PartitionKey(feedback.CustomerId));
        }
    }
}
