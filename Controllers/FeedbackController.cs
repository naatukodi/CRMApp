using Microsoft.AspNetCore.Mvc;
using CRMApp.Repository;
using CRMApp.Models;
using Microsoft.Azure.Cosmos;

namespace CRMApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FeedbackController : ControllerBase
    {
        private readonly CRMRepository _repository;

        public FeedbackController(CRMRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        public async Task<IActionResult> AddFeedback([FromBody] Feedback feedback)
        {
            await _repository.AddDocumentAsync(feedback);
            return Ok(new { message = "Feedback submitted successfully!" });
        }

        [HttpGet("{customerId}")]
        public async Task<IActionResult> GetFeedbackByCustomer(string customerId)
        {
            var feedbacks = await _repository.GetFeedbackByCustomerAsync(customerId);
            return Ok(feedbacks);
        }
    }
}
