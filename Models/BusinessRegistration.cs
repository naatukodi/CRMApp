using System;

namespace CRMApp.Models
{
    public class BusinessRegistration
    {
        public string id { get; set; } = Guid.NewGuid().ToString();
        public string? customerId { get; set; }
        public string Type { get; set; } = "Business"; // Document type identifier
        public string? BusinessName { get; set; }
        public string? OwnerName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? GstNumber { get; set; }
        public string? BusinessType { get; set; }
        public string? BusinessSubtype { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Zipcode { get; set; }
    }
}
