using System.Text.Json.Serialization;
using Rocket.Api.Contracts.Connectors;

namespace Rocket.Microsoft.Contracts
{
    public class MicrosoftConnectorSpecifics : ConnectorSummary
    {
        [JsonPropertyName("client_id")]
        public string ClientId { get; set; }
        
        [JsonPropertyName("tenant_id")]
        public string TenantId { get; set; }
    }
}