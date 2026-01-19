using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using Rocket.Domain.Vendors.Dropbox;
using Rocket.Domain.Vendors.Temporary;

namespace Rocket.Domain.Executions
{
    [BsonDiscriminator(RootClass = true)] 
    [BsonKnownTypes(typeof(DropboxUploadExecutionStep))]
    [BsonKnownTypes(typeof(EmailFileAttachmentExecutionStep))]
    public abstract record BaseExecutionStep
    {
        public string Id { get; set; }
        
        public string ConnectionId { get; set; }

        public int InputType { get; set; }
        
        public int OutputType { get; set; }
        
        public string StepName { get; set; }
        
        public DateTime? RunDate { get; set; }
        
        public int ExecutionStatus { get; set; }
        
        public IEnumerable<BaseExecutionStep> ChildSteps { get; set; }
    }
}