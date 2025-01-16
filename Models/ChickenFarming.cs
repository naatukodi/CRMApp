using System;

namespace CRMApp.Models
{
    public class ChickenFarming
    {
        public string id { get; set; } = Guid.NewGuid().ToString();
        public string Type { get; set; } = "ChickenFarming"; // Document type identifier
        public string? CustomerId { get; set; }
        public string? FarmerName { get; set; }
        public string? PhoneOrEmail { get; set; }
        public string? FarmingType { get; set; }
        public string? ChickenCount { get; set; }
        public string? FeedType { get; set; }
        public string? FarmingExperience { get; set; }
        public string? Challenges { get; set; }
        public string? SupportNeeded { get; set; }
    }
}
