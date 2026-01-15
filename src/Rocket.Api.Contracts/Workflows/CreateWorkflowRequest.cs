using System.Text.Json.Serialization;

namespace Rocket.Api.Contracts.Workflows
{
    public class CreateWorkflowRequest
    {
        [JsonPropertyName("matching_page_symbol")]
        public int? MatchingPageSymbol { get; set; }
        
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}