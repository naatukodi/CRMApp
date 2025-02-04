namespace CRMApp.Models;
using System.ComponentModel.DataAnnotations;

public class Customer
{
    public string id { get; set; } = Guid.NewGuid().ToString();
    public string customerId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string BusinessType { get; set; }
    public string Notes { get; set; }
}
