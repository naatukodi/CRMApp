using CRMApp.Models;
using CRMApp.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CRMApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BusinessRegistrationController : ControllerBase
    {
        private readonly BusinessRegistrationRepository _repository;

        public BusinessRegistrationController(BusinessRegistrationRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        public async Task<IActionResult> RegisterBusiness([FromBody] BusinessRegistration business)
        {
            if (business == null || string.IsNullOrEmpty(business.BusinessName) || string.IsNullOrEmpty(business.GstNumber))
            {
                return BadRequest(new { message = "Invalid business registration data." });
            }

            await _repository.AddBusinessAsync(business);
            return Ok(new { message = "Business registered successfully!" });
        }
    }
}