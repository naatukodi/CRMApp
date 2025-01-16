using CRMApp.Models;
using Microsoft.Azure.Cosmos;
using System.Threading.Tasks;

namespace CRMApp.Repository
{
    public class BusinessQuestionnaireRepository
    {
        private readonly Container _container;

        public BusinessQuestionnaireRepository(CosmosClient cosmosClient, IConfiguration configuration)
        {
            var databaseName = configuration["CosmosDb:DatabaseName"];
            var containerName = configuration["CosmosDb:ContainerName"];
            _container = cosmosClient.GetContainer(databaseName, containerName);
        }

        public async Task AddFeedbackAsync(BusinessQuestionnaire feedback)
        {
            await _container.CreateItemAsync(feedback, new PartitionKey(feedback.CustomerId));
        }
    }
}
