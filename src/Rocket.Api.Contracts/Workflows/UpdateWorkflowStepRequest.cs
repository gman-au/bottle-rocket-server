using System.Text.Json.Serialization;

namespace Rocket.Api.Contracts.Workflows
{
    public class UpdateWorkflowStepRequest<T> where T : WorkflowStepSummary
    {
        [JsonPropertyName("workflow_id")]
        public string WorkflowId { get; set; }

        [JsonPropertyName("workflow_step_id")]
        public string WorkflowStepId { get; set; }

        [JsonPropertyName("connection_id")]
        public string ConnectionId { get; set; }

        [JsonPropertyName("step")]
        public T Step { get; set; }
    }
}