using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Rocket.Domain
{
    [BsonIgnoreExtraElements]
    public record GlobalSettings
    {
        [BsonId]
        public ObjectId Id { get; set; }

        public int SweepSuccessfulScansAfterDays { get; set; }

        public bool EnableSweeping { get; set; }

        public int? DefaultModelTimeoutInMinutes { get; set; }
    }
}