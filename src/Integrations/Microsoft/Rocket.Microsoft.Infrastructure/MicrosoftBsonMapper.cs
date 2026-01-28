using MongoDB.Bson.Serialization;
using Rocket.Interfaces;
using Rocket.Microsoft.Domain;

namespace Rocket.Microsoft.Infrastructure
{
    public class MicrosoftBsonMapper : IBsonMapper
    {
        public void MapApplicableBsonTypes()
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(MicrosoftConnector)))
            {
                BsonClassMap.RegisterClassMap<MicrosoftConnector>(cm =>
                {
                    cm.AutoMap();
                });
            }
            
            if (!BsonClassMap.IsClassMapRegistered(typeof(OneDriveUploadExecutionStep)))
            {
                BsonClassMap.RegisterClassMap<OneDriveUploadExecutionStep>(cm =>
                {
                    cm.AutoMap();
                });
            }
            
            if (!BsonClassMap.IsClassMapRegistered(typeof(OneDriveUploadWorkflowStep)))
            {
                BsonClassMap.RegisterClassMap<OneDriveUploadWorkflowStep>(cm =>
                {
                    cm.AutoMap();
                });
            }
        }
    }
}