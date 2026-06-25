using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Rocket.Api.Contracts.Dashboard
{
    public class ExecutionsResponse
    {       
        [JsonPropertyName("total_executions")]
        public int? TotalExecutions { get; set; }
        
        [JsonPropertyName("executions_by_status")]
        public IEnumerable<ExecutionByStatusTotalSpecifics> ExecutionsByStatus { get; set; }
        
        [JsonPropertyName("executions_by_workflow")]
        public IEnumerable<ExecutionByWorkflowTotalSpecifics> ExecutionsByWorkflow { get; set; }
    }
}