using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Rocket.Api.Contracts.Executions
{
    public class ExecutionSummary : ApiResponse
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        
        [JsonPropertyName("user_id")]
        public string UserId { get; set; }

        [JsonPropertyName("scan_id")]
        public string ScanId { get; set; }
        
        [JsonPropertyName("matching_page_symbol")]
        public int? MatchingPageSymbol { get; set; }
        
        [JsonPropertyName("created_at")]
        public DateTime? CreatedAt { get; set; }
        
        [JsonPropertyName("run_date")]
        public DateTime? RunDate { get; set; }
        
        [JsonPropertyName("name")]
        public string Name { get; set; }
        
        [JsonPropertyName("execution_status")]
        public int? ExecutionStatus { get; set; }
        
        [JsonPropertyName("steps")]
        public IEnumerable<ExecutionStepSummary> Steps { get; set; }
    }
}