using MongoDB.Bson.Serialization;
using Rocket.Interfaces;
using Rocket.Replicate.Domain.Models.DataLabTo;

namespace Rocket.Replicate.Infrastructure.Models.DataLabTo
{
    public class DataLabToBsonMapper : IBsonMapper
    {
        public void MapApplicableBsonTypes()
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(DataLabToExtractTextExecutionStep)))
            {
                BsonClassMap.RegisterClassMap<DataLabToExtractTextExecutionStep>(cm =>
                {
                    cm.AutoMap();
                });
            }
            
            if (!BsonClassMap.IsClassMapRegistered(typeof(DataLabToExtractTextWorkflowStep)))
            {
                BsonClassMap.RegisterClassMap<DataLabToExtractTextWorkflowStep>(cm =>
                {
                    cm.AutoMap();
                });
            }
        }
    }
}