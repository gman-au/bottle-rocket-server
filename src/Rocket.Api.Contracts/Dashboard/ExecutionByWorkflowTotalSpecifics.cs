using System.Text.Json.Serialization;

namespace Rocket.Api.Contracts.Dashboard
{
    public class ExecutionByWorkflowTotalSpecifics
    {
        [JsonPropertyName("workflow")]
        public string Workflow { get; set; }
        
        [JsonPropertyName("executions")]
        public int Executions { get; set; }
    }
}