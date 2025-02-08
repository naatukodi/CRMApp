using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using CRMApp.Models;
using CRMApp.Repository; // Your existing CosmosDB service namespace

namespace CRMApp.Repository
{
    public class VetSupportRepository : IVetSupportRepository
    {
        private readonly ICosmosDbService _cosmosDbService;
        private const string ContainerName = "VetSupportRequests";

        public VetSupportRepository(ICosmosDbService cosmosDbService)
        {
            _cosmosDbService = cosmosDbService;
        }

        public async Task<IEnumerable<VetSupportRequest>> GetAllRequestsAsync()
        {
            var query = new QueryDefinition("SELECT * FROM c WHERE c.type = 'VetSupportRequest'");
            return await _cosmosDbService.GetItemsAsync<VetSupportRequest>(query);
        }

        public async Task<VetSupportRequest> GetRequestByIdAsync(string id, string customerId)
        {
            return await _cosmosDbService.GetItemAsync<VetSupportRequest>(id, customerId);
        }

         public async Task<List<VetSupportRequest>> GetVetSupportsByCustomerIdAsync(string customerId)
        {
            var query = new QueryDefinition(
                "SELECT * FROM c WHERE c.type = 'VetSupportRequest' AND c.customerId = @customerId"
            ).WithParameter("@customerId", customerId);

            return await _cosmosDbService.GetItemsAsync<VetSupportRequest>(query);
        }

        public async Task AddRequestAsync(VetSupportRequest request)
        {
            await _cosmosDbService.AddItemAsync(request, request.CustomerId);
        }
    }
}
