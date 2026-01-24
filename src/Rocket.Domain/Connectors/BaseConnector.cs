using MongoDB.Bson.Serialization.Attributes;
using Rocket.Domain.Core;
using Rocket.Dropbox.Domain;
using Rocket.MaxOcr.Domain;

namespace Rocket.Domain.Connectors
{
    [BsonDiscriminator(RootClass = true)]
    [BsonKnownTypes(typeof(DropboxConnector))]
    [BsonKnownTypes(typeof(MaxOcrConnector))]
    public abstract record BaseConnector : CoreExecutionStep;
}