using CRMApp.Models;
using CRMApp.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CRMApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BusinessFeedbackController : ControllerBase
    {
        private readonly BusinessFeedbackRepository _repository;

        public BusinessFeedbackController(BusinessFeedbackRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        public async Task<IActionResult> SubmitFeedback([FromBody] BusinessFeedback feedback)
        {
            if (feedback == null || string.IsNullOrEmpty(feedback.BusinessName))
            {
                return BadRequest(new { message = "Invalid feedback data." });
            }

            await _repository.AddFeedbackAsync(feedback);
            return Ok(new { message = "Feedback submitted successfully!" });
        }
    }
}
