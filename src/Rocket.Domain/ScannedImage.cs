using System;

namespace Rocket.Domain
{
    public class ScannedImage
    {
        public DateTime CaptureDate { get; set; }
        
        public string BlobId { get; set; }
        
        public string ContentType { get; set; }
        
        public string FileExtension { get; set; }
        
        public string Sha256 { get; set; }
    }
}