using MongoDB.Bson.Serialization;
using Rocket.Gcp.Domain;
using Rocket.Infrastructure.Encryption;
using Rocket.Interfaces;

namespace Rocket.Gcp.Infrastructure
{
    public class GcpBsonMapper(
        EncryptedStringSerializer enc
    ) : IBsonMapper
    {
        public void MapApplicableBsonTypes()
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(GcpConnector)))
            {
                BsonClassMap.RegisterClassMap<GcpConnector>(cm =>
                {
                    cm.AutoMap();
                });                
                
                BsonClassMap.RegisterClassMap<GcpCredential>(cm =>
                {
                    cm.AutoMap();
                    cm.GetMemberMap(o => o.PrivateKeyId)
                        .SetSerializer(enc);
                    cm.GetMemberMap(o => o.PrivateKey)
                        .SetSerializer(enc);
                    cm.GetMemberMap(o => o.ClientId)
                        .SetSerializer(enc);
                });
            }
            
            if (!BsonClassMap.IsClassMapRegistered(typeof(GcpExtractExecutionStep)))
            {
                BsonClassMap.RegisterClassMap<GcpExtractExecutionStep>(cm =>
                {
                    cm.AutoMap();
                });
            }
            
            if (!BsonClassMap.IsClassMapRegistered(typeof(GcpExtractWorkflowStep)))
            {
                BsonClassMap.RegisterClassMap<GcpExtractWorkflowStep>(cm =>
                {
                    cm.AutoMap();
                });
            }
        }
    }
}