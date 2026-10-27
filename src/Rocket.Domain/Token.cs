using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Rocket.Domain
{
    public record Token
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        
        public string UserId { get; set; }
        
        public string TokenPrefix { get; set; }
        
        public string DeviceName { get; set; }
        
        // TODO: make this a Mongo index
        public string Hash { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        public DateTime? LastLoginAt { get; set; }
        
        public bool IsActive { get; set; }
        
        public bool IsAdmin { get; set; }
        
        public bool DarkMode { get; set; }
    }
}