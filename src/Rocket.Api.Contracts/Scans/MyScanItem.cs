using System;
using System.Text.Json.Serialization;

namespace Rocket.Api.Contracts.Scans
{
    public class MyScanItem
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        
        [JsonPropertyName("date_scanned")]
        public DateTime DateScanned { get; set; }
        
        [JsonPropertyName("content_type")]
        public string ContentType { get; set; }
        
        [JsonPropertyName("thumbnail_base64")]
        public string ThumbnailBase64 { get; set; }
    }
}