using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Rocket.Domain
{
    public record Workflow
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        
        public string UserId { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        public DateTime? LastUpdatedAt { get; set; }
        
        public string Name { get; set; }
        
        public bool IsActive { get; set; }
    }
}