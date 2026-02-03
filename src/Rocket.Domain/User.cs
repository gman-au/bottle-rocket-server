using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Rocket.Domain
{
    public record User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        
        public string Username { get; set; }
        
        public string PasswordHash { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        public DateTime? LastLoginAt { get; set; }
        
        public bool IsActive { get; set; }
        
        public bool IsAdmin { get; set; }
        
        public bool DarkMode { get; set; }
    }
}