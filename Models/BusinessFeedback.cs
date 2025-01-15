using System;
using System.Collections.Generic;

namespace CRMApp.Models
{
    public class BusinessFeedback
    {
        public string id { get; set; } = Guid.NewGuid().ToString(); // Unique identifier
        public string Type { get; set; } = "BusinessFeedback"; // Document type identifier
        public string? CustomerId { get; set; } // Partition Key
        public string? BusinessName { get; set; }
        public string? ContactPerson { get; set; }
        public string? PhoneOrEmail { get; set; }
        public string? Location { get; set; }
        public string? BusinessType { get; set; }
        public string? PurchaseFrequency { get; set; }
        public List<string>? ChickenProducts { get; set; }
        public string? AverageOrderSize { get; set; }
        public string? Qualities { get; set; }
        public bool FreeRange { get; set; }
        public bool FrozenChicken { get; set; }
        public bool DeliveryPreference { get; set; }
    }
}
