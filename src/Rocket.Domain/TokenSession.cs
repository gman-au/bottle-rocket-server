using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Rocket.Domain
{
    public record TokenSession
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        
        public string UserId { get; set; }
        
        public bool IsAdmin { get; set; }
        
        public string TokenPrefix { get; set; }
        
        public string Hash { get; set; }
        
        [BsonIgnoreIfNull]
        public string PreviousHash { get; set; } // for reuse detection
        
        public DateTime? RotatedAt { get; set; }
        
        public string DeviceName { get; set; }
        
        public string UserAgent { get; set; }
        
        public string IpAddress { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        public DateTime? LastLoginAt { get; set; }
        
        public DateTime ExpiresAt { get; set; }    // sliding refresh expiry
        
        public DateTime? RevokedAt { get; set; }
        
        public bool IsActive { get; set; }
    }
}