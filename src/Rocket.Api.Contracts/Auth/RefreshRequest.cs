using System.Text.Json.Serialization;

namespace Rocket.Api.Contracts.Auth
{
    public class RefreshRequest
    {
        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; set; }
    }
}