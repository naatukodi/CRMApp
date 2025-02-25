using System;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using CRMApp.Models;

namespace CRMApp.Repository
{
    public class RetailSurveyRepository : IRetailSurveyRepository
    {
        private readonly Container _container;

        public RetailSurveyRepository(CosmosClient cosmosClient, IConfiguration configuration)
        {
            var databaseName = configuration["CosmosDb:DatabaseName"];
            var containerName = configuration["CosmosDb:ContainerName"];
            _container = cosmosClient.GetContainer(databaseName, containerName);
        }

        public async Task CreateRetailSurveyAsync(RetailSurvey survey)
        {
            survey.Id = Guid.NewGuid().ToString();
            await _container.CreateItemAsync(survey, new PartitionKey(survey.customerId));
        }
    }
}
