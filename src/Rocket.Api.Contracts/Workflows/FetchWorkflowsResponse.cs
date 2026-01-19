using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Rocket.Api.Contracts.Workflows
{
    public class FetchWorkflowsResponse : ApiResponse
    {
        [JsonPropertyName("workflows")]
        public IEnumerable<MyWorkflowSummary> Workflows { get; set; }
        
        [JsonPropertyName("total_records")]
        public int TotalRecords { get; set; }
    }
}