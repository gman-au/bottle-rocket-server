using System.Text.Json.Serialization;

namespace Rocket.Api.Contracts.Users
{
    public class UpdateUserResponse : ApiResponse
    {
        [JsonPropertyName("qr_code_base64")]
        public string QrCodeBase64 { get; set; }
    }
}