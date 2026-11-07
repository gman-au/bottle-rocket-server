using System;
using System.Text.Json.Serialization;

namespace Rocket.Api.Contracts.Batches
{
    public class BatchSummary
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        
        [JsonPropertyName("date_scanned")]
        public DateTime DateScanned { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }
        
        [JsonPropertyName("scan_count")]
        public int ScanCount { get; set; }
        
        [JsonPropertyName("cover_content_type")]
        public string CoverContentType { get; set; }
        
        [JsonPropertyName("cover_thumbnail_base64")]
        public string CoverThumbnailBase64 { get; set; }
    }
}