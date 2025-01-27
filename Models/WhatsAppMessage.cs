using System;
using System.Text.Json.Serialization;

namespace CRMApp.Models
{
    public class WhatsAppMessage
    {
        [JsonPropertyName("from")]
        public string? From { get; set; }

        [JsonPropertyName("to")]
        public string? To { get; set; }

        [JsonPropertyName("body")]
        public string? Body { get; set; }

        [JsonPropertyName("interactive_reply")]
        public InteractiveReply? InteractiveReply { get; set; }
    }

    public class InteractiveReply
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("title")]
        public string? Title { get; set; }
    }
}
