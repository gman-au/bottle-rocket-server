using System.Text.Json.Serialization;

namespace Rocket.Api.Contracts.Workflows
{
    public class UpdateWorkflowStepRequest<T> where T : WorkflowStepSummary
    {
        [JsonPropertyName("workflow_id")]
        public string WorkflowId { get; set; }

        [JsonPropertyName("workflow_step_id")]
        public string WorkflowStepId { get; set; }

        [JsonPropertyName("connector_id")]
        public string ConnectorId { get; set; }

        [JsonPropertyName("step")]
        public T Step { get; set; }
    }
}