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

        // Method that returns the first matching farmer or null if none exists.
        public async Task<FarmerRegistration?> GetFarmerByPhoneNumberAsync(string phoneNumber)
        {
            var queryDefinition = new QueryDefinition("SELECT TOP 1 * FROM c WHERE c.PhoneNumber = @phoneNumber")
                .WithParameter("@phoneNumber", phoneNumber);

            using (FeedIterator<FarmerRegistration> resultSet = _container.GetItemQueryIterator<FarmerRegistration>(queryDefinition))
            {
                while (resultSet.HasMoreResults)
                {
                    var response = await resultSet.ReadNextAsync();
                    var farmer = response.FirstOrDefault();
                    if (farmer != null)
                    {
                        return farmer;
                    }
                }
            }
            return null;
        }
    }
}
