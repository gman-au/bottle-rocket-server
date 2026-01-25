using System.Text.Json.Serialization;
using Rocket.Api.Contracts.Connectors;

namespace Rocket.Ollama.Contracts
{
    public class OllamaConnectorSpecifics : ConnectorSummary
    {
        [JsonPropertyName("endpoint")]
        public string Endpoint { get; set; }
    }
}