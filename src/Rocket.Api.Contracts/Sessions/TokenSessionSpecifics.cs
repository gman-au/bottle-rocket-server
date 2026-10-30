using System;
using System.Text.Json.Serialization;

namespace Rocket.Api.Contracts.Sessions
{
    public class TokenSessionSpecifics
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("device_name")]
        public string DeviceName { get; set; }

        [JsonPropertyName("user_agent")]
        public string UserAgent { get; set; }

        [JsonPropertyName("ip_address")]
        public string IpAddress { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("last_login_at")]
        public DateTime? LastLoginAt { get; set; }

        [JsonPropertyName("is_current")]
        public bool IsCurrent { get; set; }

        [JsonPropertyName("token_prefix")]
        public string TokenPrefix { get; set; }
    }
}