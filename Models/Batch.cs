using System.Text.Json.Serialization;
namespace CRMApp.Models;

public class Batch
{
    [JsonPropertyName("id")]
    public string id { get; set; } // Unique Batch ID

    [JsonPropertyName("type")]
    public string Type { get; set; } = "batch"; // Document type

    [JsonPropertyName("CustomerId")]
    public string? CustomerId { get; set; } // User ID for partitioning

    [JsonPropertyName("batchId")]
    public string? batchId { get; set; }

    [JsonPropertyName("dateCreated")]
    public DateTime? DateCreated { get; set; }

    [JsonPropertyName("totalChickens")]
    public int? TotalChickens { get; set; }

    [JsonPropertyName("active")]
    public bool Active { get; set; } // Indicates if the batch is currently active

    [JsonPropertyName("completed")]
    public bool Completed { get; set; } // Indicates if the batch is completed

    [JsonPropertyName("feedDetails")]
    public List<FeedDetails> FeedDetails { get; set; } = new(); // Feed information

    [JsonPropertyName("chickens")]
    public List<ChickenDetails> Chickens { get; set; } = new();
}

public class FeedDetails
{
    [JsonPropertyName("feedType")]
    public string? FeedType { get; set; } // Type of feed

    [JsonPropertyName("weightFed")]
    public double? WeightFed { get; set; } // Weight of feed in kilograms

    [JsonPropertyName("timeOfDay")]
    public string? TimeOfDay { get; set; } // Morning, Afternoon, Evening

    [JsonPropertyName("dateFed")]
    public DateTime? DateFed { get; set; } // Date of feeding
}

public class ChickenDetails
{
    [JsonPropertyName("id")]
    public string id { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; } = "chicken";

    [JsonPropertyName("CustomerId")]
    public string? CustomerId { get; set; }

    [JsonPropertyName("chickenId")]
    public string? ChickenId { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("breed")]
    public string? Breed { get; set; }

    [JsonPropertyName("dateOfBirth")]
    public DateTime? DateOfBirth { get; set; }

    [JsonPropertyName("status")]
    public string? Status { get; set; } = "alive";
}