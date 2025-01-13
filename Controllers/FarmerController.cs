using CRMApp.Models;
using CRMApp.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CRMApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FarmerController : ControllerBase
    {
        private readonly FarmerQuestionnaireRepository _repository;

        public FarmerController(FarmerQuestionnaireRepository repository)
        {
            _repository = repository;
        }

        [HttpPost("submit")]
        public async Task<IActionResult> SubmitQuestionnaire([FromBody] FarmerQuestionnaire questionnaire)
        {
            if (questionnaire == null || string.IsNullOrEmpty(questionnaire.CustomerId))
            {
                return BadRequest(new { message = "Questionnaire data or CustomerId is missing." });
            }

            await _repository.AddQuestionnaireAsync(questionnaire);
            return Ok(new { message = "Questionnaire submitted successfully!", data = questionnaire });
        }

        [HttpGet("all/{customerId}")]
        public async Task<IActionResult> GetAllQuestionnaires(string customerId)
        {
            var questionnaires = await _repository.GetAllQuestionnairesAsync(customerId);
            return Ok(questionnaires);
        }

        [HttpGet("{id}/{customerId}")]
        public async Task<IActionResult> GetQuestionnaireById(string id, string customerId)
        {
            var questionnaire = await _repository.GetQuestionnaireByIdAsync(id, customerId);
            if (questionnaire == null)
            {
                return NotFound(new { message = "Questionnaire not found." });
            }
            return Ok(questionnaire);
        }
    }
}