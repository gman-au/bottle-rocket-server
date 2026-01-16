using System.Text.Json.Serialization;

namespace Rocket.Api.Contracts.Workflows
{
    public class DeleteWorkflowStepRequest
    {
        [JsonPropertyName("workflow_step_id")]
        public string WorkflowStepId { get; set; }

        [JsonPropertyName("workflow_id")]
        public string WorkflowId { get; set; }
    }
}