using MongoDB.Bson.Serialization;
using Rocket.Interfaces;
using Rocket.Replicate.Domain.Models.DeepSeekOcr;

namespace Rocket.Replicate.Infrastructure.Models.DeepSeekOcr
{
    public class DeepSeekBsonMapper : IBsonMapper
    {
        public void MapApplicableBsonTypes()
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(DeepSeekOcrExtractTextExecutionStep)))
            {
                BsonClassMap.RegisterClassMap<DeepSeekOcrExtractTextExecutionStep>(cm =>
                {
                    cm.AutoMap();
                });
            }
            
            if (!BsonClassMap.IsClassMapRegistered(typeof(DeepSeekOcrExtractTextWorkflowStep)))
            {
                BsonClassMap.RegisterClassMap<DeepSeekOcrExtractTextWorkflowStep>(cm =>
                {
                    cm.AutoMap();
                });
            }
        }
    }
}