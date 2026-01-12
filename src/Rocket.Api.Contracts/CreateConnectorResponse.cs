using System.Text.Json.Serialization;

namespace Rocket.Api.Contracts
{
    public class CreateConnectorResponse : ApiResponse
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("created_at")]
        public System.DateTime CreatedAt { get; set; }
    }
}