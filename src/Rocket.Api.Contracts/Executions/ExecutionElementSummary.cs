using System.Text.Json.Serialization;

namespace Rocket.Api.Contracts.Executions
{
    public class ExecutionElementSummary : ApiResponse
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        
        [JsonPropertyName("scan_id")]
        public string ScanId { get; set; }
        
        [JsonPropertyName("execution_status")]
        public int ExecutionStatus { get; set; }
        
        [JsonPropertyName("log_messages")]
        public string[] LogMessages { get; set; }
        
        [JsonPropertyName("thumbnail_base64")]
        public string ThumbnailBase64 { get; set; }
        
        [JsonPropertyName("content_type")]
        public string ContentType { get; set; }
        
        [JsonPropertyName("vendor")]
        public string Vendor { get; set; }
    }
}