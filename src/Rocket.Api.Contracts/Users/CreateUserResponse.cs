using System.Text.Json.Serialization;

namespace Rocket.Api.Contracts.Users
{
    public class CreateUserResponse : ApiResponse
    {
        [JsonPropertyName("user_name")]
        public string Username { get; set; }

        [JsonPropertyName("created_at")]
        public System.DateTime CreatedAt { get; set; }
        
        [JsonPropertyName("qr_code_base64")]
        public string QrCodeBase64 { get; set; }
    }
}