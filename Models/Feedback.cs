namespace CRMApp.Models;
public class Feedback
{
    public string id { get; set; } = Guid.NewGuid().ToString();
    public string customerId { get; set; }
    public string FeedbackMessage { get; set; }
    public string Status { get; set; } = "Pending"; // Default value
}