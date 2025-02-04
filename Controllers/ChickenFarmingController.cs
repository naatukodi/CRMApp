using CRMApp.Models;
using CRMApp.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CRMApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChickenFarmingController : ControllerBase
    {
        private readonly ChickenFarmingRepository _repository;

        public ChickenFarmingController(ChickenFarmingRepository repository)
        {
            _repository = repository;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterChickenFarming([FromBody] ChickenFarming farming)
        {
            if (farming == null || string.IsNullOrEmpty(farming.customerId))
            {
                return BadRequest(new { message = "Invalid chicken farming registration data." });
            }

            await _repository.AddChickenFarmingAsync(farming);
            return Ok(new { message = "Chicken Farming registered successfully!" });
        }

        [HttpPost("feedback")]
        public async Task<IActionResult> SubmitFeedback([FromBody] FarmerFeedback feedback)
        {
            if (feedback == null || string.IsNullOrEmpty(feedback.customerId))
            {
                return BadRequest(new { message = "Invalid feedback data." });
            }

            await _repository.AddFarmerFeedbackAsync(feedback);
            return Ok(new { message = "Feedback submitted successfully!" });
        }
    }
}