using System.Text.Json.Serialization;

namespace Rocket.Api.Contracts
{
    public class ConnectionTestResponse : ApiResponse
    {
        [JsonPropertyName("user_name")]
        public string UserName { get; set; }
        
        [JsonPropertyName("role")]
        public string Role { get; set; }
    }
}