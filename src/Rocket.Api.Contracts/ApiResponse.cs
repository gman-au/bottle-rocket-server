using System.Text.Json.Serialization;

namespace Rocket.Api.Contracts
{
    public class ApiResponse
    {
        [JsonPropertyName("error_code")] public int ErrorCode { get; set; }
        
        [JsonPropertyName("error_message")] public string ErrorMessage { get; set; }
    }
}