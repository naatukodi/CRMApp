// Dtos/ContactMessageDto.cs
using System.ComponentModel.DataAnnotations;

namespace CRMApp.Dtos
{
    public class ContactMessageDto
    {
        [Required]
        public string Name { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string customerId { get; set; }

        // Phone represents the customer id
        [Required]
        public string Phone { get; set; }

        [Required]
        public string Message { get; set; }
    }
}
