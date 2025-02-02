using System.Text.Json.Serialization;
namespace CRMApp.Models;

public class BatchSummary
{
    [JsonPropertyName("batchId")]
    public string? BatchId { get; set; } // Unique Batch ID

    [JsonPropertyName("dateCreated")]
    public DateTime? DateCreated { get; set; } // Date when the batch was created

    [JsonPropertyName("chickenCount")]
    public int? ChickenCount { get; set; } // Total number of chickens in the batch
}
