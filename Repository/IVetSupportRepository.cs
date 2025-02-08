using System.Collections.Generic;
using System.Threading.Tasks;
using CRMApp.Models;

namespace CRMApp.Repository
{
    public interface IVetSupportRepository
    {
        Task<IEnumerable<VetSupportRequest>> GetAllRequestsAsync();
        Task<VetSupportRequest> GetRequestByIdAsync(string id, string customerId);
        Task<List<VetSupportRequest>> GetVetSupportsByCustomerIdAsync(string customerId);
        Task AddRequestAsync(VetSupportRequest request);
    }
}
