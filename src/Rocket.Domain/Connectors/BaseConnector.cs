using MongoDB.Bson.Serialization.Attributes;
using Rocket.Domain.Core;
using Rocket.Dropbox.Domain;

namespace Rocket.Domain.Connectors
{
    [BsonDiscriminator(RootClass = true)]
    [BsonKnownTypes(typeof(DropboxConnector))]
    public abstract record BaseConnector : CoreExecutionStep;
}