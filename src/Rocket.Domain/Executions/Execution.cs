using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Rocket.Domain.Executions
{
    public record Execution
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        
        public string UserId { get; set; }
        
        public string WorkflowId { get; set; }
        
        public int? MatchingPageSymbol { get; set; }
        
        public DateTime? RunDate { get; set; }
        
        public string Name { get; set; }
        
        public int ExecutionStatus { get; set; }
        
        public IEnumerable<BaseExecutionStep> Steps { get; set; }
    }
}