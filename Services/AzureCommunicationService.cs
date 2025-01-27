using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace CRMApp.Services
{
    public class AzureCommunicationService : IAzureCommunicationService
    {
        private readonly HttpClient _httpClient;
        private readonly string? _acsEndpoint;
        private readonly string? _acsAccessKey;

        public AzureCommunicationService(IConfiguration configuration)
        {
            _httpClient = new HttpClient();
            _acsEndpoint = configuration["ACS:Endpoint"];
            _acsAccessKey = configuration["ACS:AccessKey"];
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

            _httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _acsAccessKey);

            var response = await _httpClient.PostAsync($"{_acsEndpoint}/messages", jsonContent);
            response.EnsureSuccessStatusCode();
        }

        public async Task SendInteractiveWhatsAppMessage(object messagePayload)
        {
            var jsonContent = new StringContent(JsonSerializer.Serialize(messagePayload), Encoding.UTF8, "application/json");

            _httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _acsAccessKey);

            var response = await _httpClient.PostAsync($"{_acsEndpoint}/messages", jsonContent);
            response.EnsureSuccessStatusCode();
        }
    }
}
