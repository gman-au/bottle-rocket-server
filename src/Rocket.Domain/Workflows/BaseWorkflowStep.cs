using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using Rocket.Domain.Vendors.Dropbox;
using Rocket.Domain.Vendors.Temporary;

namespace Rocket.Domain.Workflows
{
    [BsonDiscriminator(RootClass = true)] 
    [BsonKnownTypes(typeof(DropboxUploadWorkflowStep))]
    [BsonKnownTypes(typeof(EmailFileAttachmentWorkflowStep))]
    public abstract record BaseWorkflowStep
    {
        public string Id { get; set; }
        
        public string ConnectionId { get; set; }

        public abstract int InputType { get; set; }
        
        public abstract int OutputType { get; set; }
        
        public abstract string StepName { get; set; }
        
        public IEnumerable<BaseWorkflowStep> ChildSteps { get; set; }
    }
}