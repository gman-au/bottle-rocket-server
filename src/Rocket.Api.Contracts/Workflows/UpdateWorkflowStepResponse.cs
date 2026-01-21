using System.Text.Json.Serialization;

namespace Rocket.Api.Contracts.Workflows
{
    public class UpdateWorkflowStepResponse : ApiResponse
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
    }
}