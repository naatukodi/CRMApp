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
            if (farmer == null || string.IsNullOrEmpty(farmer.Name) || string.IsNullOrEmpty(farmer.customerId))
            {
                return BadRequest(new { message = "Invalid farmer registration data." });
            }

            await _repository.AddFarmerAsync(farmer);
            return Ok(new { message = "Farmer registered successfully!" });
        }

        [HttpGet("check")]
        public async Task<IActionResult> CheckFarmerRegistration([FromQuery] string phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber))
            {
                return BadRequest(new { message = "Phone number is required." });
            }

            FarmerRegistration? farmer = await _repository.GetFarmerByPhoneNumberAsync(phoneNumber);

            if (farmer != null)
            {
                return Ok(new { status = "registered", farmer });
            }
            else
            {
                return Ok(new { status = "notregistered", farmer = (FarmerRegistration?)null });
            }
        }
    }
}
