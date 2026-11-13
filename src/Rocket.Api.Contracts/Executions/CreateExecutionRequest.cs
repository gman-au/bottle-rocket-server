using System.Text.Json.Serialization;

namespace Rocket.Api.Contracts.Executions
{
    public class CreateExecutionRequest
    {
        [JsonPropertyName("workflow_id")]
        public string WorkflowId { get; set; }

        [JsonPropertyName("batch_id")]
        public string BatchId { get; set; }

        [JsonPropertyName("run_immediately")]
        public bool? RunImmediately { get; set; }
    }
}