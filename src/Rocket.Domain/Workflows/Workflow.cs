using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Rocket.Domain.Core;

namespace Rocket.Domain.Workflows
{
    public record Workflow
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        
        public string UserId { get; set; }
        
        public int? MatchingPageSymbol { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        public DateTime? LastUpdatedAt { get; set; }
        
        public string Name { get; set; }
        
        public bool IsActive { get; set; }
        
        public IEnumerable<CoreWorkflowStep> Steps { get; set; }
    }
}