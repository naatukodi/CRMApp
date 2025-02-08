using System;
using Newtonsoft.Json;

namespace CRMApp.Models
{
    public class VetSupportRequest
    {
        [JsonProperty("id")]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [JsonProperty("type")]
        public string Type { get; set; } = "VetSupportRequest"; // CosmosDB Document Type Identifier

        [JsonProperty("chickenId")]
        public string? ChickenId { get; set; }

        [JsonProperty("customerId")]
        public string? CustomerId { get; set; }

        [JsonProperty("farmerName")]
        public string? FarmerName { get; set; }

        [JsonProperty("phoneNumber")]
        public string? PhoneNumber { get; set; }

        [JsonProperty("farmAddress")]
        public string? FarmAddress { get; set; }

        [JsonProperty("farmCity")]
        public string? FarmCity { get; set; }

        [JsonProperty("farmState")]
        public string? FarmState { get; set; }

        [JsonProperty("sickChickenLocation")]
        public string? SickChickenLocation { get; set; }

        [JsonProperty("symptoms")]
        public string? Symptoms { get; set; }

        [JsonProperty("requestDate")]
        public DateTime RequestDate { get; set; } = DateTime.UtcNow;
    }
}
