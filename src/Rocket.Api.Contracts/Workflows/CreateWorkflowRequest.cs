using System.Text.Json.Serialization;

namespace Rocket.Api.Contracts.Workflows
{
    public class CreateWorkflowRequest
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}