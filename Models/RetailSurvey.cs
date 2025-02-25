using Newtonsoft.Json;
namespace CRMApp.Models;

public class RetailSurvey
{
    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("customerId")]
    public string customerId { get; set; }

    public string Type { get; set; } = "retailsurvey";

    // Section 1
    public string AgeGroup { get; set; }
    public string MonthlyIncome { get; set; }
    public string Lifestyle { get; set; }
    public string PurchaseFrequency { get; set; }

    // Section 2
    public string OrganicAwareness { get; set; }
    public string ChickenPurchaseFrequency { get; set; }
    public PurchaseDrivers PurchaseDrivers { get; set; }

    // Section 3
    public string Transparency { get; set; }
    public string QRIntegration { get; set; }
    public ProductVarieties ProductVarieties { get; set; }

    // Section 4
    public string PricePointAcceptance { get; set; }
    public string QualityResponse { get; set; }
    public string PerceivedValue { get; set; }

    // Section 5
    public string Suggestions { get; set; }
    public string FinalComments { get; set; }
}

public class PurchaseDrivers
{
    public bool Price { get; set; }
    public bool OrganicCertification { get; set; }
    public bool AnimalWelfare { get; set; }
    public bool Traceability { get; set; }
    public bool TasteQuality { get; set; }
    public bool Variety { get; set; }
    public string Other { get; set; }
}

public class ProductVarieties
{
    public bool Authentic { get; set; }
    public bool Giriraj { get; set; }
    public bool Sonali { get; set; }
    public bool NotSure { get; set; }
}
