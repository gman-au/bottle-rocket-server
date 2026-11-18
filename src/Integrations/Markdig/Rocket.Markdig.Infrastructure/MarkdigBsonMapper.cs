using MongoDB.Bson.Serialization;
using Rocket.Interfaces;
using Rocket.Markdig.Domain;

namespace Rocket.Markdig.Infrastructure
{
    public class MarkdigBsonMapper : IBsonMapper
    {
        public void MapApplicableBsonTypes()
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(ConvertToEpubExecutionStep)))
            {
                BsonClassMap.RegisterClassMap<ConvertToEpubExecutionStep>(cm =>
                {
                    cm.AutoMap();
                });
            }
            
            if (!BsonClassMap.IsClassMapRegistered(typeof(ConvertToEpubWorkflowStep)))
            {
                BsonClassMap.RegisterClassMap<ConvertToEpubWorkflowStep>(cm =>
                {
                    cm.AutoMap();
                });
            }
        }
    }
}