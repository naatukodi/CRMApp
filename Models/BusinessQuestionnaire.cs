using System;
using System.Collections.Generic;

namespace CRMApp.Models
{
    public class BusinessQuestionnaire
    {
        public string id { get; set; } = Guid.NewGuid().ToString(); // Unique identifier
        public string Type { get; set; } = "BusinessQuestionnaire"; // Document type identifier
        public string? customerId { get; set; } // Partition Key
        public string? BusinessName { get; set; }
        public string? ContactPerson { get; set; }
        public string? PhoneNumber { get; set; }
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
