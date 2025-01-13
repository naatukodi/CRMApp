using Microsoft.AspNetCore.Mvc;
using CRMApp.Repository;
using CRMApp.Models;
using Microsoft.Azure.Cosmos;

namespace CRMApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly CRMRepository _repository;

        public CustomerController(CRMRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        public async Task<IActionResult> AddCustomer([FromBody] Customer customer)
        {
            await _repository.AddDocumentAsync(customer);
            return Ok(new { message = "Customer added successfully!" });
        }

        [HttpGet]   
        public IActionResult GetAllCustomers()
        {
            var customers = _repository.GetAllCustomersAsync().Result;
            return Ok(customers);
        }
    }
}
