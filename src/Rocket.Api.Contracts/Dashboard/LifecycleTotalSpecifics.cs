using System.Text.Json.Serialization;

namespace Rocket.Api.Contracts.Dashboard
{
    public class LifecycleTotalSpecifics
    {
        [JsonPropertyName("workflow")]
        public string Workflow { get; set; }
        
        [JsonPropertyName("status")]
        public string Status { get; set; }
        
        [JsonPropertyName("count")]
        public int Count { get; set; }
    }
}