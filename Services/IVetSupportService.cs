using System.Collections.Generic;
using System.Threading.Tasks;
using CRMApp.Models;

namespace CRMApp.Services
{
    public interface IVetSupportService
    {
        Task<IEnumerable<VetSupportRequest>> GetAllRequestsAsync();
        Task<VetSupportRequest> GetRequestByIdAsync(string id, string customerId);
        Task AddRequestAsync(VetSupportRequest request);
        Task<List<VetSupportRequest>> GetVetSupportsByCustomerIdAsync(string customerId);
    }
}
