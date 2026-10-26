using System.Text.Json.Serialization;

namespace Rocket.Api.Contracts.Workflows
{
    public class CloneWorkflowRequest
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}