using MongoDB.Bson.Serialization;
using Rocket.Interfaces;
using Rocket.Notion.Domain;

namespace Rocket.Notion.Infrastructure
{
    public class NotionBsonMapper : IBsonMapper
    {
        public void MapApplicableBsonTypes()
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(NotionConnector)))
            {
                BsonClassMap.RegisterClassMap<NotionConnector>(cm =>
                {
                    cm.AutoMap();
                });
            }
            
            if (!BsonClassMap.IsClassMapRegistered(typeof(NotionUploadExecutionStep)))
            {
                BsonClassMap.RegisterClassMap<NotionUploadExecutionStep>(cm =>
                {
                    cm.AutoMap();
                });
            }
            
            if (!BsonClassMap.IsClassMapRegistered(typeof(NotionUploadWorkflowStep)))
            {
                BsonClassMap.RegisterClassMap<NotionUploadWorkflowStep>(cm =>
                {
                    cm.AutoMap();
                });
            }
        }
    }
}