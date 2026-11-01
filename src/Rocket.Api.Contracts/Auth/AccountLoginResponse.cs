using System;
using System.Text.Json.Serialization;

namespace Rocket.Api.Contracts.Auth
{
    public class AccountLoginResponse : ApiResponse
    {
        [JsonPropertyName("user_name")]
        public string UserName { get; set; }
        
        [JsonPropertyName("role")]
        public string Role { get; set; }
        
        [JsonPropertyName("dark_mode")]
        public bool DarkMode { get; set; }
        
        [JsonPropertyName("current_language")]
        public string CurrentLanguage { get; set; }        
        
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }        
        
        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; set; }
        
        [JsonPropertyName("session_id")]
        public string SessionId { get; set; }    
        
        [JsonPropertyName("access_token_expires_at")]
        public DateTime AccessTokenExpiresAt { get; set; }
    }
}