using System.Collections.Generic;
using System.Text.Json.Serialization;
using Rocket.Api.Contracts.Workflows;

namespace Rocket.Api.Contracts.Executions
{
    public class FetchExecutionsResponse : ApiResponse
    {
        [JsonPropertyName("executions")]
        public IEnumerable<ExecutionSummary> Executions { get; set; }
        
        [JsonPropertyName("total_records")]
        public int TotalRecords { get; set; }
    }
}