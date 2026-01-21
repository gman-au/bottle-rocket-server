using System;
using System.Text.Json.Serialization;

namespace Rocket.Api.Contracts.Connectors
{
    public class ConnectorSummary
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        
        [JsonPropertyName("connector_type")]
        public string ConnectorType { get; set; }
        
        [JsonPropertyName("connector_name")]
        public string ConnectorName { get; set; }
        
        [JsonPropertyName("created_at")]
        public DateTime? CreatedAt { get; set; }
        
        [JsonPropertyName("last_updated_at")]
        public DateTime? LastUpdatedAt { get; set; }
        
        [JsonPropertyName("status")]
        public int Status { get; set; }
    }
}