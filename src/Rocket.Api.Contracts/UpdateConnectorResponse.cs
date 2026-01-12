using System.Text.Json.Serialization;

namespace Rocket.Api.Contracts
{
    public class UpdateConnectorResponse : ApiResponse
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("last_updated_at")]
        public System.DateTime LastUpdatedAt { get; set; }
    }
}