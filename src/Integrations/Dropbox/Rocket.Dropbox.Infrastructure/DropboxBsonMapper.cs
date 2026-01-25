using MongoDB.Bson.Serialization;
using Rocket.Dropbox.Domain;
using Rocket.Interfaces;

namespace Rocket.Dropbox.Infrastructure
{
    public class DropboxBsonMapper : IBsonMapper
    {
        public void MapApplicableBsonTypes()
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(DropboxConnector)))
            {
                BsonClassMap.RegisterClassMap<DropboxConnector>(cm =>
                {
                    cm.AutoMap();
                });
            }
            
            if (!BsonClassMap.IsClassMapRegistered(typeof(DropboxUploadExecutionStep)))
            {
                BsonClassMap.RegisterClassMap<DropboxUploadExecutionStep>(cm =>
                {
                    cm.AutoMap();
                });
            }
            
            if (!BsonClassMap.IsClassMapRegistered(typeof(DropboxUploadWorkflowStep)))
            {
                BsonClassMap.RegisterClassMap<DropboxUploadWorkflowStep>(cm =>
                {
                    cm.AutoMap();
                });
            }
        }
    }
}