using System.Text.Json.Serialization;

namespace Rocket.Api.Contracts
{
    public class CreateUserRequest
    {
        [JsonPropertyName("user_name")]
        public string Username { get; set; }
        
        [JsonPropertyName("password")]
        public string Password { get; set; }
        
        [JsonPropertyName("is_the_new_admin")]
        public bool IsTheNewAdmin { get; set; }
    }
}