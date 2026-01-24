using System.Text.Json.Serialization;

namespace Rocket.Api.Contracts.Connectors
{
    public class CreateConnectorRequest<T> where T : ConnectorSummary
    {
        [JsonPropertyName("connector")]
        public T Connector { get; set; }
    }
}