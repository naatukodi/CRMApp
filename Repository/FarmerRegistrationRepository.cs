using CRMApp.Models;
using Microsoft.Azure.Cosmos;
using System.Threading.Tasks;

namespace CRMApp.Repository
{
    public class FarmerRegistrationRepository
    {
        private readonly Container _container;

        public FarmerRegistrationRepository(CosmosClient cosmosClient, IConfiguration configuration)
        {
            var databaseName = configuration["CosmosDb:DatabaseName"];
            var containerName = configuration["CosmosDb:ContainerName"];
            _container = cosmosClient.GetContainer(databaseName, containerName);
        }

        public async Task AddFarmerAsync(FarmerRegistration farmer)
        {
            await _container.CreateItemAsync(farmer, new PartitionKey(farmer.CustomerId));
        }
    }
}
