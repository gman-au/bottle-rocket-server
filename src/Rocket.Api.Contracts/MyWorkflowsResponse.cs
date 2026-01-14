using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Rocket.Api.Contracts
{
    public class MyWorkflowsResponse : ApiResponse
    {
        [JsonPropertyName("workflows")]
        public IEnumerable<MyWorkflowItem> Workflows { get; set; }
        
        [JsonPropertyName("total_records")]
        public int TotalRecords { get; set; }
    }
}