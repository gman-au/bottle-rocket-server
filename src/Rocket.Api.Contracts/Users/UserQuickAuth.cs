using System.Text.Json.Serialization;

namespace Rocket.Api.Contracts.Users
{
    public class UserQuickAuth
    {
        [JsonPropertyName("username")]
        public string UserName { get; set; }
        
        [JsonPropertyName("password")]
        public string Password { get; set; }
        
        [JsonPropertyName("server")]
        public string Server { get; set; }
    }
}