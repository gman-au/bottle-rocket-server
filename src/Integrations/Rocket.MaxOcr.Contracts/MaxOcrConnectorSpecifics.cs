using System.Text.Json.Serialization;
using Rocket.Api.Contracts.Connectors;

namespace Rocket.MaxOcr.Contracts
{
    public class MaxOcrConnectorSpecifics : ConnectorSummary
    {
        [JsonPropertyName("endpoint")]
        public string Endpoint { get; set; }
    }
}