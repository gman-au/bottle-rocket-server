using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Rocket.Domain.Enum;

namespace Rocket.Domain.Connectors
{
    [BsonDiscriminator(RootClass = true)] 
    [BsonKnownTypes(typeof(DropboxConnector))]
    public abstract class BaseConnector
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        
        public string UserId { get; set; }
        
        public abstract int ConnectorType { get; set; }
        
        public abstract string ConnectorName { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        public DateTime? LastUpdatedAt { get; set; }

        public abstract ConnectorStatusEnum DetermineStatus();
    }
}