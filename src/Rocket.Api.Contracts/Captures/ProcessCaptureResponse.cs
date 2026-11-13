using System.Text.Json.Serialization;

namespace Rocket.Api.Contracts.Captures
{
    public class ProcessCaptureResponse : ApiResponse
    {
        [JsonPropertyName("scan_id")]
        public string ScanId { get; set; }
        
        [JsonPropertyName("batch_id")]
        public string BatchId { get; set; }
    }
}