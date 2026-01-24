using MongoDB.Bson.Serialization.Attributes;
using Rocket.Domain.Core;
using Rocket.Domain.Vendors.Temporary;
using Rocket.Dropbox.Domain;

namespace Rocket.Domain.Executions
{
    [BsonDiscriminator(RootClass = true)]
    [BsonKnownTypes(typeof(DropboxUploadExecutionStep))]
    [BsonKnownTypes(typeof(EmailFileAttachmentExecutionStep))]
    public abstract record BaseExecutionStep : CoreExecutionStep;
}