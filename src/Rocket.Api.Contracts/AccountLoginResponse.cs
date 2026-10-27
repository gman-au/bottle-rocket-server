using System.Text.Json.Serialization;

namespace Rocket.Api.Contracts
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
        
        [JsonPropertyName("token")]
        public string Token { get; set; }
    }
}