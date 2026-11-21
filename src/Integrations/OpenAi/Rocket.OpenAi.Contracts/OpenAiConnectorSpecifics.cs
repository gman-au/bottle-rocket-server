using System.Text.Json.Serialization;
using Rocket.Api.Contracts.Connectors;

namespace Rocket.OpenAi.Contracts
{
    public class OpenAiConnectorSpecifics : ConnectorSummary
    {
        [JsonPropertyName("api_key")]
        public string ApiKey { get; set; }
    }
}