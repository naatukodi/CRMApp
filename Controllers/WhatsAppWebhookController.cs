using Microsoft.AspNetCore.Mvc;
using CRMApp.Services;
using CRMApp.Models;
using System.Text.Json;

[ApiController]
[Route("api/whatsapp")]
public class WhatsAppWebhookController : ControllerBase
{
    private readonly IAzureCommunicationService _acsService;

    public WhatsAppWebhookController(IAzureCommunicationService acsService)
    {
        _acsService = acsService;
    }

    [HttpPost("webhook")]
    public async Task<IActionResult> HandleEventGridWebhook([FromBody] JsonElement payload)
    {
        // Check if this is an Event Grid Subscription Validation request
        if (payload.TryGetProperty("eventType", out var eventTypeProp) &&
            eventTypeProp.GetString() == "Microsoft.EventGrid.SubscriptionValidationEvent")
        {
            var validationCode = payload.GetProperty("data").GetProperty("validationCode").GetString();
            Console.WriteLine($"Validation Code Received: {validationCode}");

            return Ok(new { validationResponse = validationCode });
        }

        // Handle regular WhatsApp messages
        WhatsAppMessage message;
        try
        {
            message = JsonSerializer.Deserialize<WhatsAppMessage>(payload.GetRawText());
        }
        catch
        {
            return BadRequest("Invalid message format.");
        }

        if (message == null || string.IsNullOrEmpty(message.Body) || string.IsNullOrEmpty(message.From))
            return BadRequest("Invalid message received.");

        string responseText = @"👋 Welcome to Naatukodi by GSR!  
        Please select an option by replying with a number:

        1️⃣ Register as a Farmer (రెజిస్ట్రేషన్)  
        2️⃣ Order Chicks (కోడి పిల్లల ఆర్డర్)  
        3️⃣ Request Vet Support (పశువైద్య సహాయం)  
        4️⃣ Sell Chickens (కోడి అమ్మకం)  
        5️⃣ Check Market Prices (మార్కెట్ ధరలు)";

        await _acsService.SendWhatsAppMessage(message.From, responseText);
        return Ok();
    }
}
