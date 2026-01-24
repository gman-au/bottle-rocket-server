using MongoDB.Bson.Serialization.Attributes;
using Rocket.Domain.Core;
using Rocket.Domain.Vendors.Temporary;
using Rocket.Dropbox.Domain;
using Rocket.MaxOcr.Domain;

namespace Rocket.Domain.Workflows
{
    [BsonDiscriminator(RootClass = true)]
    [BsonKnownTypes(typeof(DropboxUploadWorkflowStep))]
    [BsonKnownTypes(typeof(MaxOcrExtractWorkflowStep))]
    [BsonKnownTypes(typeof(EmailFileAttachmentWorkflowStep))]
    public abstract record BaseWorkflowStep : CoreWorkflowStep;
}