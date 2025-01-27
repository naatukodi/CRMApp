using Microsoft.Extensions.Options;
using CRMApp.Models;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CRMApp.Services
{
    public class AcsService : IAcsService
    {
        private readonly HttpClient _httpClient;
        private readonly AcsSettings _acsSettings;

        public AcsService(IOptions<AcsSettings> acsSettings)
        {
            _httpClient = new HttpClient();
            _acsSettings = acsSettings.Value;
        }

        public async Task SendWhatsAppMessage(string phoneNumber, string messageText)
        {
            var requestBody = new
            {
                channel = "whatsapp",
                recipient = new { phoneNumber = phoneNumber },
                message = new { content = new { text = messageText } }
            };

            var jsonContent = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

            _httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _acsSettings.AccessKey);

            var response = await _httpClient.PostAsync($"{_acsSettings.Endpoint}/messages", jsonContent);
            response.EnsureSuccessStatusCode();
        }

        // ✅ Implement the missing method for interactive WhatsApp messages
        public async Task SendInteractiveWhatsAppMessage(object messagePayload)
        {
            var jsonContent = new StringContent(JsonSerializer.Serialize(messagePayload), Encoding.UTF8, "application/json");

            _httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _acsSettings.AccessKey);

            var response = await _httpClient.PostAsync($"{_acsSettings.Endpoint}/messages", jsonContent);
            response.EnsureSuccessStatusCode();
        }
    }
}
