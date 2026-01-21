using System.Text.Json.Serialization;

namespace Rocket.Api.Contracts.Workflows
{
    public class CreateWorkflowResponse : ApiResponse
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
    }
}