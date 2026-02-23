using System.Text.Json.Serialization;
using Rocket.Api.Contracts.Connectors;

namespace Rocket.Replicate.Contracts
{
    public class ReplicateConnectorSpecifics : ConnectorSummary
    {
        [JsonPropertyName("api_token")]
        public string ApiToken { get; set; }
    }
}