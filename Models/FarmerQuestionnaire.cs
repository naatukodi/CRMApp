using System.Collections.Generic;
namespace CRMApp.Models;
using System.ComponentModel.DataAnnotations;

public class FarmerQuestionnaire
{
    public string id { get; set; } = Guid.NewGuid().ToString(); // Unique identifier
    public string Type { get; set; } = "FarmerQuestionnaire"; // Document type identifier
    public string? customerId { get; set; } // Partition Key
    public string? Name { get; set; }
    public string? Location { get; set; }
    public string? ContactInfo { get; set; }
    public string? ChickenBreeds { get; set; }
    public string? RearingMethod { get; set; }
    public string? ChickenCount { get; set; }
    public string? MonthlySales { get; set; }
    public List<string>? FeedMaterials { get; set; }
    public List<string>? HealthPractices { get; set; }
    public bool? HasVetAccess { get; set; }
    public List<string>? SellingMethods { get; set; }
    public bool SupplyDirectlyToMarket { get; set; }
    public string? MarketRequirements { get; set; }
    public bool InterestedInBuyBack { get; set; }
    public string? BuyBackChickenCount { get; set; }
    public bool InterestedInVetSupport { get; set; }
    public List<string>? VetSupportType { get; set; }
    public List<string>? AssistanceRequired { get; set; }
    public bool FacingChallenges { get; set; }
    public string? ChallengeDetails { get; set; }
    public bool ExploringNewMethods { get; set; }
    public bool GettingFairPrice { get; set; }
    public string? PriceIssues { get; set; }
    public string? Suggestions { get; set; }
}

