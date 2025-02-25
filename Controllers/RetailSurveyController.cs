using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using CRMApp.Services;
using CRMApp.Models;
using CRMApp.Repository;

[ApiController]
[Route("api/[controller]")]
public class RetailSurveyController : ControllerBase
{
    private readonly IRetailSurveyRepository _repository;

    public RetailSurveyController(IRetailSurveyRepository repository)
    {
        _repository = repository;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] RetailSurvey survey)
    {
        if (survey == null)
            return BadRequest("Invalid data.");

        await _repository.CreateRetailSurveyAsync(survey);
        return Ok(new { message = "Survey submitted successfully" });
    }
}
