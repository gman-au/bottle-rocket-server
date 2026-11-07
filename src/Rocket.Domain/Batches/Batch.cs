using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Rocket.Domain.ScannedImages;

namespace Rocket.Domain.Batches
{
    public record Batch
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        
        public string UserId { get; set; }
        
        public DateTime CaptureDate { get; set; }

        public int Status { get; set; }
        
        public string Description { get; set; }

        public bool Archived { get; set; }
        
        public IEnumerable<ScannedImage> Scans { get; set; }
    }
}