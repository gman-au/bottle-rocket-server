using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using Rocket.Domain.Workflows;

namespace Rocket.Domain.Executions
{
    [BsonDiscriminator(RootClass = true)] 
    [BsonKnownTypes(typeof(DropboxUploadStep))]
    [BsonKnownTypes(typeof(EmailFileAttachmentStep))]
    public abstract record BaseExecutionStep
    {
        public string Id { get; set; }
        
        public string ConnectionId { get; set; }

        public abstract int InputType { get; set; }
        
        public abstract int OutputType { get; set; }
        
        public abstract string StepName { get; set; }
        
        public DateTime RunDate { get; set; }
        
        public int ExecutionStatus { get; set; }
        
        public IEnumerable<BaseExecutionStep> ChildSteps { get; set; }
    }
}