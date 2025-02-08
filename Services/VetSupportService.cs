using System.Collections.Generic;
using System.Threading.Tasks;
using CRMApp.Models;
using CRMApp.Repository;

namespace CRMApp.Services
{
    public class VetSupportService : IVetSupportService
    {
        private readonly IVetSupportRepository _repository;

        public VetSupportService(IVetSupportRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<VetSupportRequest>> GetAllRequestsAsync()
        {
            return await _repository.GetAllRequestsAsync();
        }

        public async Task<VetSupportRequest> GetRequestByIdAsync(string id, string customerId)
        {
            return await _repository.GetRequestByIdAsync(id, customerId);
        }

        public async Task<List<VetSupportRequest>> GetVetSupportsByCustomerIdAsync(string customerId)
        {
            return await _repository.GetVetSupportsByCustomerIdAsync(customerId);
        }

        public async Task AddRequestAsync(VetSupportRequest request)
        {
            await _repository.AddRequestAsync(request);
        }
    }
}
