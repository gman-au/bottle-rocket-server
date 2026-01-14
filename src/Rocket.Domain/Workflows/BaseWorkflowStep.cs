using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Rocket.Domain.Workflows
{
    [BsonDiscriminator(RootClass = true)] 
    [BsonKnownTypes(typeof(DropboxUploadStep))]
    public abstract record BaseWorkflowStep
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        
        public string ConnectionId { get; set; }
        
        public abstract int InputType { get; set; }
        
        public abstract int OutputType { get; set; }
        
        public abstract string StepName { get; set; }
        
        public IEnumerable<BaseWorkflowStep> ChildSteps { get; set; }
    }
}