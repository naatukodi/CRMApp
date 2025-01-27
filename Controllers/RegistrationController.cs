using Microsoft.AspNetCore.Mvc;
using CRMApp.Services;
using CRMApp.Models;

[ApiController]
[Route("api/whatsapp")]
public class RegistrationController : ControllerBase
{
    private readonly IAcsService _acsService;

    public RegistrationController(IAcsService acsService)
    {
        _acsService = acsService;
    }
    [HttpPost("registration")]
    public async Task<IActionResult> RegistrationMessage([FromBody] WhatsAppMessage message)
    {
        if (message == null || string.IsNullOrEmpty(message.From))
        {
            return BadRequest("Invalid message or phone number.");
        }
        string phoneNumber = message.From; // Extract farmer's phone number
        string registrationUrl = $"https://your-angular-app.com/register/{phoneNumber}";

        var response = new
        {
            channel = "whatsapp",
            recipient = new { phoneNumber = phoneNumber },
            message = new
            {
                content = new
                {
                    interactive = new
                    {
                        type = "button",
                        body = new { text = "Click below to register: 👇" },
                        action = new
                        {
                            buttons = new[]
                            {
                                new { type = "url", text = "📝 Register Now", url = registrationUrl }
                            }
                        }
                    }
                }
            }
        };

        await _acsService.SendInteractiveWhatsAppMessage(response);
        return Ok();
    }
}
