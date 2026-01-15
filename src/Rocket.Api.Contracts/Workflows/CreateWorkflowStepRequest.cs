using System.Text.Json.Serialization;

namespace Rocket.Api.Contracts.Workflows
{
    public class CreateWorkflowStepRequest<T> where T : WorkflowStepDetail
    {
        [JsonPropertyName("workflow_id")]
        public string WorkflowId { get; set; }

        [JsonPropertyName("parent_step_id")]
        public string ParentStepId { get; set; }

        [JsonPropertyName("connection_id")]
        public string ConnectionId { get; set; }

        [JsonPropertyName("step")]
        public T Step { get; set; }
    }
}