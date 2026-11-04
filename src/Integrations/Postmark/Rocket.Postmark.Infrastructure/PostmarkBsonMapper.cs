using MongoDB.Bson.Serialization;
using Rocket.Infrastructure.Encryption;
using Rocket.Interfaces;
using Rocket.Postmark.Domain;

namespace Rocket.Postmark.Infrastructure
{
    public class PostmarkBsonMapper(
        EncryptedStringSerializer enc
    ) : IBsonMapper
    {
        public void MapApplicableBsonTypes()
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(PostmarkConnector)))
            {
                BsonClassMap.RegisterClassMap<PostmarkConnector>(cm =>
                {
                    cm.AutoMap();
                    cm.GetMemberMap(o => o.ServerToken)
                        .SetSerializer(enc);
                });
            }
            
            if (!BsonClassMap.IsClassMapRegistered(typeof(PostmarkSendEmailExecutionStep)))
            {
                BsonClassMap.RegisterClassMap<PostmarkSendEmailExecutionStep>(cm =>
                {
                    cm.AutoMap();
                });
            }
            
            if (!BsonClassMap.IsClassMapRegistered(typeof(PostmarkSendEmailWorkflowStep)))
            {
                BsonClassMap.RegisterClassMap<PostmarkSendEmailWorkflowStep>(cm =>
                {
                    cm.AutoMap();
                });
            }
        }
    }
}