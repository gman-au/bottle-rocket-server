using System.Text.Json.Serialization;

namespace Rocket.Api.Contracts.Auth
{
    public class AccountLoginRequest
    {
        [JsonPropertyName("user_name")]
        public string Username { get; set; }

        [JsonPropertyName("password")]
        public string Password { get; set; }
        
        [JsonPropertyName("device_name")]
        public string DeviceName { get; set; }

        [JsonPropertyName("use_cookie")]
        public bool UseCookie { get; set; }
    }
}