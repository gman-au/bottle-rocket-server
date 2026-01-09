using System;
using System.Text.Json.Serialization;

namespace Rocket.Api.Contracts
{
    public class UserDetail : ApiResponse
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        
        [JsonPropertyName("user_name")]
        public string Username { get; set; }
        
        [JsonPropertyName("created_at")]
        public DateTime? CreatedAt { get; set; }
        
        [JsonPropertyName("last_login_at")]
        public DateTime? LastLoginAt { get; set; }
        
        [JsonPropertyName("is_active")]
        public bool? IsActive { get; set; }
        
        [JsonPropertyName("is_admin")]
        public bool? IsAdmin { get; set; }
        
        [JsonPropertyName("new_password")]
        public string NewPassword { get; set; }
    }
}