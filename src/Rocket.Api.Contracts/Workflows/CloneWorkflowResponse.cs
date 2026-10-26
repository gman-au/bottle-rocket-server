using System.Text.Json.Serialization;

namespace Rocket.Api.Contracts.Workflows
{
    public class CloneWorkflowResponse : ApiResponse
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
    }
}