using CRMApp.Models;
using CRMApp.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CRMApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FarmerRegistrationController : ControllerBase
    {
        private readonly FarmerRegistrationRepository _repository;

        public FarmerRegistrationController(FarmerRegistrationRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        public async Task<IActionResult> RegisterFarmer([FromBody] FarmerRegistration farmer)
        {
            if (farmer == null || string.IsNullOrEmpty(farmer.FullName) || string.IsNullOrEmpty(farmer.CustomerId))
            {
                return BadRequest(new { message = "Invalid farmer registration data." });
            }

            await _repository.AddFarmerAsync(farmer);
            return Ok(new { message = "Farmer registered successfully!" });
        }
    }
}
