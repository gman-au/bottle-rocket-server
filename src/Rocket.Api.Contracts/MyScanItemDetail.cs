using System;
using System.Text.Json.Serialization;

namespace Rocket.Api.Contracts
{
    public class MyScanItemDetail : ApiResponse
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        
        [JsonPropertyName("user_id")]
        public string UserId { get; set; }

        [JsonPropertyName("capture_date")]
        public DateTime? CaptureDate { get; set; }
        
        [JsonPropertyName("blob_id")]
        public string BlobId { get; set; }
        
        [JsonPropertyName("content_type")]
        public string ContentType { get; set; }
        
        [JsonPropertyName("file_extension")]
        public string FileExtension { get; set; }
        
        [JsonPropertyName("sha_256")]
        public string Sha256 { get; set; }
        
        [JsonPropertyName("image_base64")]
        public string ImageBase64 { get; set; }
    }
}