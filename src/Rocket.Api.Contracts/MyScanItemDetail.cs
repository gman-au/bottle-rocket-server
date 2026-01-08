using System;
using System.Text.Json.Serialization;

namespace Rocket.Api.Contracts
{
    public class MyScanItemDetail : ApiResponse
    {
        public string Id { get; set; }
        public string UserId { get; set; }

        //[JsonPropertyName("capture_date")]
        public DateTime? CaptureDate { get; set; }
        
        public string BlobId { get; set; }
        
        public string ContentType { get; set; }
        
        public string FileExtension { get; set; }
        
        public string Sha256 { get; set; }
        
        public string ImageBase64 { get; set; }
    }
}