using MongoDB.Bson.Serialization;
using Rocket.Interfaces;
using Rocket.Postmark.Domain;

namespace Rocket.Postmark.Infrastructure
{
    public class PostmarkBsonMapper : IBsonMapper
    {
        public void MapApplicableBsonTypes()
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(PostmarkConnector)))
            {
                BsonClassMap.RegisterClassMap<PostmarkConnector>(cm =>
                {
                    cm.AutoMap();
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