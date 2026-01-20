using System.Text.Json.Serialization;

namespace Rocket.Api.Contracts.Captures
{
    public class ProcessCaptureResponse : ApiResponse
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
    }
}