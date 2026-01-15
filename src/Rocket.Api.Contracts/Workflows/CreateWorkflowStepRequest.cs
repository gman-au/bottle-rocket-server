using System.Text.Json.Serialization;

namespace Rocket.Api.Contracts.Workflows
{
    public class CreateWorkflowStepRequest
    {
        [JsonPropertyName("workflow_id")]
        public string WorkflowId { get; set; }

        [JsonPropertyName("parent_step_id")]
        public string ParentStepId { get; set; }
    }
}