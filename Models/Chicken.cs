using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System;

namespace CRMApp.Models;

public class Chicken
{
    [JsonPropertyName("id")]
    public string id { get; set; } // Cosmos DB requires lowercase 'id'

    [JsonPropertyName("chickenId")]
    public string ChickenId { get; set; } // Uppercase first letter for consistency

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("breed")]
    public string? Breed { get; set; }

    [JsonPropertyName("weight")]
    public double? Weight { get; set; }

    [JsonPropertyName("dateOfBirth")]
    public DateTime? DateOfBirth { get; set; }

    [JsonPropertyName("CustomerId")]
    public string? CustomerId { get; set; } // Partition key, lowercase for Cosmos DB compatibility

    [JsonPropertyName("healthRecords")]
    public List<HealthRecord> HealthRecords { get; set; } = new();

    [JsonPropertyName("batchId")]
    public string? batchId { get; set; } // Associated Batch ID

    [JsonPropertyName("status")]
    public string? Status { get; set; } // "alive", "processed", or "deceased"

}

public class HealthRecord
{
    [JsonPropertyName("date")]
    public DateTime Date { get; set; }

    [JsonPropertyName("weight")]
    public double Weight { get; set; }

    [JsonPropertyName("appetite")]
    public bool Appetite { get; set; }

    [JsonPropertyName("vaccinated")]
    public bool Vaccinated { get; set; }

    [JsonPropertyName("notes")]
    public string? Notes { get; set; }
}
