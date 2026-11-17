using MongoDB.Bson.Serialization;
using Rocket.Interfaces;
using Rocket.Replicate.Domain.Models.Gpt;

namespace Rocket.Replicate.Infrastructure.Models.Gpt
{
    public class GptBsonMapper : IBsonMapper
    {
        public void MapApplicableBsonTypes()
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(GptRetitleFileExecutionStep)))
            {
                BsonClassMap.RegisterClassMap<GptRetitleFileExecutionStep>(cm =>
                {
                    cm.AutoMap();
                });
            }
            
            if (!BsonClassMap.IsClassMapRegistered(typeof(GptRetitleFileWorkflowStep)))
            {
                BsonClassMap.RegisterClassMap<GptRetitleFileWorkflowStep>(cm =>
                {
                    cm.AutoMap();
                });
            }
        }
    }
}