using CRMApp.Models;
using Microsoft.Azure.Cosmos;
using System.Threading.Tasks;

namespace CRMApp.Repository
{
    public class BusinessRegistrationRepository
    {
        private readonly Container _container;

        public BusinessRegistrationRepository(CosmosClient cosmosClient, IConfiguration configuration)
        {
            var databaseName = configuration["CosmosDb:DatabaseName"];
            var containerName = configuration["CosmosDb:ContainerName"];
            _container = cosmosClient.GetContainer(databaseName, containerName);
        }

        public async Task AddBusinessAsync(BusinessRegistration business)
        {
            await _container.CreateItemAsync(business, new PartitionKey(business.CustomerId));
        }
    }
}
