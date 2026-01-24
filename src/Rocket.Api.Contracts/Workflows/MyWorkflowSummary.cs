using System;
using System.Text.Json.Serialization;

namespace Rocket.Api.Contracts.Workflows
{
    public class MyWorkflowSummary : ApiResponse
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        
        [JsonPropertyName("user_id")]
        public string UserId { get; set; }
        
        [JsonPropertyName("matching_page_symbol")]
        public int? MatchingPageSymbol { get; set; }
        
        [JsonPropertyName("created_at")]
        public DateTime? CreatedAt { get; set; }
        
        [JsonPropertyName("last_updated_at")]
        public DateTime? LastUpdatedAt { get; set; }
        
        [JsonPropertyName("name")]
        public string Name { get; set; }
        
        [JsonPropertyName("is_active")]
        public bool? IsActive { get; set; }
    }
}