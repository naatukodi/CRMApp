// Models/ContactMessage.cs
using System;

namespace CRMApp.Models
{
    public class ContactMessage
    {
        // Cosmos DB requires an 'id' field
        public string id { get; set; }

        // Fixed type for this collection of messages
        public string type { get; set; } = "contactus";

        public string Name { get; set; }
        public string Email { get; set; }

        public string customerId { get; set; }

        // CustomerId is the phone number
        public string Phone { get; set; }

        public string Message { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
