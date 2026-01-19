using System.Text.Json.Serialization;

namespace Rocket.Api.Contracts.Executions
{
    public class CreateExecutionRequest
    {
        [JsonPropertyName("workflow_id")]
        public string WorkflowId { get; set; }
    }
}