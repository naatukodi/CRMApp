using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using CRMApp.Models;
using CRMApp.Services;

namespace CRMApp.Controllers
{
    [ApiController]
    [Route("api/vetsupport")]
    public class VetSupportController : ControllerBase
    {
        private readonly IVetSupportService _service;

        public VetSupportController(IVetSupportService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<VetSupportRequest>>> GetAllRequests()
        {
            var requests = await _service.GetAllRequestsAsync();
            return Ok(requests);
        }

        [HttpGet("{id}/{customerId}")]
        public async Task<ActionResult<VetSupportRequest>> GetRequestById(string id, string customerId)
        {
            var request = await _service.GetRequestByIdAsync(id, customerId);
            if (request == null) return NotFound();
            return Ok(request);
        }

          [HttpGet("customer/{customerId}")]
        public async Task<IActionResult> GetVetSupportRequestsByCustomerId(string customerId)
        {
            var vetRequests = await _service.GetVetSupportsByCustomerIdAsync(customerId);
            if (vetRequests == null || vetRequests.Count == 0)
            {
                return NotFound(new { message = "No vet support requests found for this customer." });
            }
            return Ok(vetRequests);
        }

        [HttpPost]
        public async Task<ActionResult> CreateRequest([FromBody] VetSupportRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            await _service.AddRequestAsync(request);
            return CreatedAtAction(nameof(GetRequestById), new { id = request.Id, customerId = request.CustomerId }, request);
        }
    }
}
