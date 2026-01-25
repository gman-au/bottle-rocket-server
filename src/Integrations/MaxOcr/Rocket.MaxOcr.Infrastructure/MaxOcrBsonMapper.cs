using MongoDB.Bson.Serialization;
using Rocket.Interfaces;
using Rocket.MaxOcr.Domain;

namespace Rocket.MaxOcr.Infrastructure
{
    public class MaxOcrBsonMapper : IBsonMapper
    {
        public void MapApplicableBsonTypes()
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(MaxOcrConnector)))
            {
                BsonClassMap.RegisterClassMap<MaxOcrConnector>(cm =>
                {
                    cm.AutoMap();
                });
            }
            
            if (!BsonClassMap.IsClassMapRegistered(typeof(MaxOcrExtractExecutionStep)))
            {
                BsonClassMap.RegisterClassMap<MaxOcrExtractExecutionStep>(cm =>
                {
                    cm.AutoMap();
                });
            }
            
            if (!BsonClassMap.IsClassMapRegistered(typeof(MaxOcrExtractWorkflowStep)))
            {
                BsonClassMap.RegisterClassMap<MaxOcrExtractWorkflowStep>(cm =>
                {
                    cm.AutoMap();
                });
            }
        }
    }
}