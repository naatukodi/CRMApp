using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Threading.Tasks;
using CRMApp.Services;
using CRMApp.Models;

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
        // Check if the payload is an array or a single event object
        if (payload.ValueKind == JsonValueKind.Array)
        {
            // Handle array of events
            foreach (var eventObj in payload.EnumerateArray())
            {
                var result = await ProcessEvent(eventObj);
                if (result != null)
                {
                    return result;
                }
            }
        }
        else if (payload.ValueKind == JsonValueKind.Object)
        {
            // Handle single event object
            var result = await ProcessEvent(payload);
            if (result != null)
            {
                return result;
            }
        }
        else
        {
            return BadRequest("Invalid payload format. Expected an array or object.");
        }

        // Return success if no errors occurred
        return Ok();
    }

    private async Task<IActionResult?> ProcessEvent(JsonElement eventObj)
    {
        // Check if the event is a subscription validation event
        if (eventObj.TryGetProperty("eventType", out var eventTypeProp) &&
            eventTypeProp.GetString() == "Microsoft.EventGrid.SubscriptionValidationEvent")
        {
            // Extract the validation code and respond
            var validationCode = eventObj.GetProperty("data").GetProperty("validationCode").GetString();
            Console.WriteLine($"Validation Code Received: {validationCode}");

            return Ok(new { validationResponse = validationCode });
        }

        // Check if the event is an advanced WhatsApp message event
        if (eventObj.TryGetProperty("eventType", out var eventType) &&
            eventType.GetString() == "Microsoft.Communication.AdvancedMessageReceived")
        {
            try
            {
                // Deserialize the advanced WhatsApp message event
                var messageData = eventObj.GetProperty("data");

                var whatsAppMessage = new WhatsAppMessage
                {
                    From = messageData.GetProperty("from").GetString(),
                    Body = messageData.GetProperty("content").GetString(),
                    ReceivedTimestamp = messageData.GetProperty("receivedTimestamp").GetString()
                };

                // Validate the message
                if (string.IsNullOrEmpty(whatsAppMessage.From) || string.IsNullOrEmpty(whatsAppMessage.Body))
                {
                    return BadRequest("Invalid WhatsApp message received.");
                }

                // Prepare the response message
                string responseText = @"👋 Welcome to Naatukodi by GSR!  
                Please select an option by replying with a number:

                1️⃣ Register as a Farmer (రెజిస్ట్రేషన్)  
                2️⃣ Order Chicks (కోడి పిల్లల ఆర్డర్)  
                3️⃣ Request Vet Support (పశువైద్య సహాయం)  
                4️⃣ Sell Chickens (కోడి అమ్మకం)  
                5️⃣ Check Market Prices (మార్కెట్ ధరలు)";

                // Send the response via Azure Communication Services
                await _acsService.SendWhatsAppMessage(whatsAppMessage.From, responseText);

                Console.WriteLine($"Processed WhatsApp message from {whatsAppMessage.From}: {whatsAppMessage.Body}");
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error deserializing WhatsApp message: {ex.Message}");
                return BadRequest("Invalid WhatsApp message format.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing WhatsApp message: {ex.Message}");
                return StatusCode(500, "An error occurred while processing the message.");
            }
        }

        // Return null if no specific action is needed
        return null;
    }
}