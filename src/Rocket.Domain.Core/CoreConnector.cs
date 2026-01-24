using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Rocket.Domain.Core.Enum;

namespace Rocket.Domain.Core
{
    public abstract record CoreConnector
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string UserId { get; set; }

        public abstract int ConnectorType { get; set; }

        public abstract string ConnectorName { get; set; }

        public abstract string ConnectorCode { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? LastUpdatedAt { get; set; }

        public abstract ConnectorStatusEnum DetermineStatus();
    }
}