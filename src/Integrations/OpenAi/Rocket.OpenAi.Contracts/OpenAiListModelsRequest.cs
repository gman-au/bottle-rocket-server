using System.Text.Json.Serialization;

namespace Rocket.OpenAi.Contracts
{
    public class OpenAiListModelsRequest
    {
        [JsonPropertyName("connector_id")]
        public string ConnectorId { get; set; }
    }
}