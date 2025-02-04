using CRMApp.Models;
using Microsoft.Azure.Cosmos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CRMApp.Repository
{
    public class FarmerQuestionnaireRepository
    {
        private readonly Container _container;

        public FarmerQuestionnaireRepository(CosmosClient cosmosClient, IConfiguration configuration)
        {
            var databaseName = configuration["CosmosDb:DatabaseName"];
            var containerName = configuration["CosmosDb:ContainerName"];
            _container = cosmosClient.GetContainer(databaseName, containerName);
        }

        // Save a new Farmer Questionnaire
        public async Task AddQuestionnaireAsync(FarmerQuestionnaire questionnaire)
        {
            if (string.IsNullOrEmpty(questionnaire.customerId))
            {
                throw new ArgumentException("customerId must be provided as it is the partition key.");
            }

            await _container.CreateItemAsync(questionnaire, new PartitionKey(questionnaire.customerId));
        }

        // Get all Farmer Questionnaires
        public async Task<List<FarmerQuestionnaire>> GetAllQuestionnairesAsync(string customerId)
        {
            var query = "SELECT * FROM c WHERE c.type = 'FarmerQuestionnaire'";
            var iterator = _container.GetItemQueryIterator<FarmerQuestionnaire>(
                query,
                requestOptions: new QueryRequestOptions
                {
                    PartitionKey = new PartitionKey(customerId)
                });

            var results = new List<FarmerQuestionnaire>();
            while (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync();
                results.AddRange(response);
            }

            return results;
        }

        // Get a Farmer Questionnaire by ID
        public async Task<FarmerQuestionnaire> GetQuestionnaireByIdAsync(string id, string customerId)
        {
            try
            {
                var response = await _container.ReadItemAsync<FarmerQuestionnaire>(id, new PartitionKey(customerId));
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null; // Return null if not found
            }
        }
    }
}