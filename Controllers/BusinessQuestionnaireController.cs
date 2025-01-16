using CRMApp.Models;
using CRMApp.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CRMApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BusinessQuestionnaireController : ControllerBase
    {
        private readonly BusinessQuestionnaireRepository _repository;

        public BusinessQuestionnaireController(BusinessQuestionnaireRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        public async Task<IActionResult> SubmitFeedback([FromBody] BusinessQuestionnaire feedback)
        {
            if (feedback == null || string.IsNullOrEmpty(feedback.BusinessName))
            {
                return BadRequest(new { message = "Invalid Questionnaire data." });
            }

            await _repository.AddFeedbackAsync(feedback);
            return Ok(new { message = "Business Questionnaire submitted successfully!" });
        }
    }
}
