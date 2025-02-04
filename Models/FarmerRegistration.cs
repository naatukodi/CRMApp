using System;

namespace CRMApp.Models
{
    public class FarmerRegistration
    {
        public string id { get; set; } = Guid.NewGuid().ToString();
        public string Type { get; set; } = "Farmer"; // Document type identifier
        public string? customerId { get; set; }
        public string? FullName { get; set; }
        public string? AadharNumber { get; set; }
        public string? PhoneNumber { get; set; }
        public string? FarmerType { get; set; }
        public string? FarmerSubtype { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Zipcode { get; set; }
    }
}
