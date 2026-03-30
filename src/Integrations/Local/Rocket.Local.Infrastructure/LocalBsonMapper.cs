using MongoDB.Bson.Serialization;
using Rocket.Interfaces;
using Rocket.Local.Domain;

namespace Rocket.Local.Infrastructure
{
    public class LocalBsonMapper : IBsonMapper
    {
        public void MapApplicableBsonTypes()
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(LocalUploadExecutionStep)))
            {
                BsonClassMap.RegisterClassMap<LocalUploadExecutionStep>(cm =>
                {
                    cm.AutoMap();
                });
            }
            
            if (!BsonClassMap.IsClassMapRegistered(typeof(LocalUploadWorkflowStep)))
            {
                BsonClassMap.RegisterClassMap<LocalUploadWorkflowStep>(cm =>
                {
                    cm.AutoMap();
                });
            }
        }
    }
}