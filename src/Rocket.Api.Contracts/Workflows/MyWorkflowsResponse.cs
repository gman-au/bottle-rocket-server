using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Rocket.Api.Contracts.Workflows
{
    public class MyWorkflowsResponse : ApiResponse
    {
        [JsonPropertyName("workflows")]
        public IEnumerable<WorkflowSummary> Workflows { get; set; }
        
        [JsonPropertyName("total_records")]
        public int TotalRecords { get; set; }
    }
}