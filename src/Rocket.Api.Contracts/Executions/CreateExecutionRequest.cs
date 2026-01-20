using System.Text.Json.Serialization;

namespace Rocket.Api.Contracts.Executions
{
    public class CreateExecutionRequest
    {
        [JsonPropertyName("workflow_id")]
        public string WorkflowId { get; set; }

        [JsonPropertyName("scan_id")]
        public string ScanId { get; set; }

        [JsonPropertyName("run_immediately")]
        public bool? RunImmediately { get; set; }
    }
}