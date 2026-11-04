using MongoDB.Bson.Serialization;
using Rocket.Infrastructure.Encryption;
using Rocket.Interfaces;
using Rocket.Replicate.Domain;

namespace Rocket.Replicate.Infrastructure
{
    public class ReplicateBsonMapper(
        EncryptedStringSerializer enc
    ) : IBsonMapper
    {
        public void MapApplicableBsonTypes()
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(ReplicateConnector)))
            {
                BsonClassMap.RegisterClassMap<ReplicateConnector>(cm =>
                    {
                        cm.AutoMap();
                        cm.GetMemberMap(o => o.ApiToken)
                            .SetSerializer(enc);
                    }
                );
            }
        }
    }
}