using System.Text.Json.Serialization;

namespace Rocket.Api.Contracts
{
    public class AccountLoginRequest
    {
        [JsonPropertyName("user_name")]
        public string Username { get; set; }

        [JsonPropertyName("password")]
        public string Password { get; set; }
    }
}