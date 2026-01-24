using System.Text.Json.Serialization;

namespace Rocket.Api.Contracts.Connectors
{
    public class CreateConnectorResponse<T> : ApiResponse where T : ConnectorSummary
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        
        [JsonPropertyName("connector")]
        public T Connector { get; set; }
    }
}