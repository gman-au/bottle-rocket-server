using System.Text.Json.Serialization;

namespace Rocket.Ollama.Contracts
{
    public class CreateOllamaConnectorRequest
    {
        [JsonPropertyName("endpoint")]
        public string Endpoint { get; set; }
    }
}