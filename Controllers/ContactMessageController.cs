// Controllers/ContactMessageController.cs
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CRMApp.Dtos;
using CRMApp.Models;
using CRMApp.Repository;

namespace CRMApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactMessageController : ControllerBase
    {
        private readonly ICosmosDbService _cosmosDbService;

        public ContactMessageController(ICosmosDbService cosmosDbService)
        {
            _cosmosDbService = cosmosDbService;
        }

        [HttpPost("submit")]
        public async Task<IActionResult> Submit([FromBody] ContactMessageDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Create a new contact message and map the DTO values.
            var contactMessage = new ContactMessage
            {
                id = Guid.NewGuid().ToString(),
                Name = dto.Name,
                Email = dto.Email,
                customerId = dto.Phone, // Use phone as customer id/partition key
                Phone = dto.Phone,
                Message = dto.Message,
                // type is automatically set to "contactus"
                CreatedAt = DateTime.UtcNow
            };

            // Use the phone number as the partition key.
            await _cosmosDbService.AddItemAsync(contactMessage, contactMessage.customerId);

            return Ok(new { message = "Message submitted successfully" });
        }
    }
}
