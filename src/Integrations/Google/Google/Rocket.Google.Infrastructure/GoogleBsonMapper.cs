using MongoDB.Bson.Serialization;
using Rocket.Google.Domain;
using Rocket.Interfaces;

namespace Rocket.Google.Infrastructure
{
    public class GoogleBsonMapper : IBsonMapper
    {
        public void MapApplicableBsonTypes()
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(GoogleConnector)))
            {
                BsonClassMap.RegisterClassMap<GoogleConnector>(cm =>
                {
                    cm.AutoMap();
                });
            }
            
            if (!BsonClassMap.IsClassMapRegistered(typeof(GoogleDriveUploadExecutionStep)))
            {
                BsonClassMap.RegisterClassMap<GoogleDriveUploadExecutionStep>(cm =>
                {
                    cm.AutoMap();
                });
            }
            
            if (!BsonClassMap.IsClassMapRegistered(typeof(GoogleDriveUploadWorkflowStep)))
            {
                BsonClassMap.RegisterClassMap<GoogleDriveUploadWorkflowStep>(cm =>
                {
                    cm.AutoMap();
                });
            }
        }
    }
}