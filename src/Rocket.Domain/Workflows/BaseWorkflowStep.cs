using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace Rocket.Domain.Workflows
{
    [BsonDiscriminator(RootClass = true)] 
    [BsonKnownTypes(typeof(DropboxUploadStep))]
    public abstract record BaseWorkflowStep
    {
        public string Id { get; set; }
        
        public string ConnectionId { get; set; }
        
        public abstract int InputType { get; }
        
        public abstract int OutputType { get; }
        
        public abstract string StepName { get; }
        
        public IEnumerable<BaseWorkflowStep> ChildSteps { get; set; }
    }
}