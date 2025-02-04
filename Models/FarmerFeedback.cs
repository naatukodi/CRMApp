using System;

namespace CRMApp.Models
{
    public class FarmerFeedback
    {
        public string id { get; set; } = Guid.NewGuid().ToString();
        public string Type { get; set; } = "FarmerFeedback";
        public string? customerId { get; set; }
        public string? FarmingExperience { get; set; }
        public string? Challenges { get; set; }
        public string? SupportNeeded { get; set; }
        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;
    }
}
