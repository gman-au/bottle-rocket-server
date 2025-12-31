using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Rocket.Domain
{
    public class ScannedImage
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        
        public string UserId { get; set; }
        
        public DateTime CaptureDate { get; set; }
        
        public string BlobId { get; set; }
        
        public string ContentType { get; set; }
        
        public string FileExtension { get; set; }
        
        public string Sha256 { get; set; }
    }
}