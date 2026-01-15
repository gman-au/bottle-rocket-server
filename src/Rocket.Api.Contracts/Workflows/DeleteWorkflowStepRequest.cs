using System.Text.Json.Serialization;

namespace Rocket.Api.Contracts.Workflows
{
    public class DeleteWorkflowStepRequest
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("workflow_id")]
        public string WorkflowId { get; set; }
    }
}