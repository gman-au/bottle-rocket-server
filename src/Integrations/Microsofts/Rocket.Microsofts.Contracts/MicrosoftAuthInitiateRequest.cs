using System.Text.Json.Serialization;

namespace Rocket.Microsofts.Contracts
{
    public class MicrosoftAuthInitiateRequest
    {
        [JsonPropertyName("client_id")]
        public string ClientId { get; set; }

        [JsonPropertyName("tenant_id")]
        public string TenantId { get; set; }
    }
}