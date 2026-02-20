using System.Text.Json.Serialization;

namespace Rocket.Api.Contracts
{
    public class VersionResponse : ApiResponse
    {
        [JsonPropertyName("api_version")]
        public string ApiVersion { get; set; }
        
        [JsonPropertyName("web_version")]
        public string WebVersion { get; set; }
    }
}