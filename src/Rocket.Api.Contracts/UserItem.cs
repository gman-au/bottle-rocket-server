using System;
using System.Text.Json.Serialization;

namespace Rocket.Api.Contracts
{
    public class UserItem
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        
        [JsonPropertyName("user_name")]
        public string Username { get; set; }
        
        [JsonPropertyName("created_at")]
        public DateTime? CreatedAt { get; set; }
    }
}