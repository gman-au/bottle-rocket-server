using MongoDB.Bson.Serialization;
using Rocket.Interfaces;
using Rocket.Replicate.Domain;

namespace Rocket.Replicate.Infrastructure
{
    public class ReplicateBsonMapper : IBsonMapper
    {
        public void MapApplicableBsonTypes()
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(ReplicateConnector)))
            {
                BsonClassMap.RegisterClassMap<ReplicateConnector>(cm =>
                {
                    cm.AutoMap();
                });
            }
        }
    }
}