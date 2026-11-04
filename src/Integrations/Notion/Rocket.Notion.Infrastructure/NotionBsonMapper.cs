using MongoDB.Bson.Serialization;
using Rocket.Infrastructure.Encryption;
using Rocket.Interfaces;
using Rocket.Notion.Domain;

namespace Rocket.Notion.Infrastructure
{
    public class NotionBsonMapper(
        EncryptedStringSerializer enc
    ) : IBsonMapper
    {
        public void MapApplicableBsonTypes()
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(NotionConnector)))
            {
                BsonClassMap.RegisterClassMap<NotionConnector>(cm =>
                    {
                        cm.AutoMap();
                        cm.GetMemberMap(o => o.IntegrationSecret)
                            .SetSerializer(enc);
                    }
                );
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(NotionUploadNoteExecutionStep)))
            {
                BsonClassMap.RegisterClassMap<NotionUploadNoteExecutionStep>(cm => { cm.AutoMap(); });
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(NotionUploadNoteWorkflowStep)))
            {
                BsonClassMap.RegisterClassMap<NotionUploadNoteWorkflowStep>(cm => { cm.AutoMap(); });
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(NotionUploadProjectTaskExecutionStep)))
            {
                BsonClassMap.RegisterClassMap<NotionUploadProjectTaskExecutionStep>(cm => { cm.AutoMap(); });
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(NotionUploadProjectTaskWorkflowStep)))
            {
                BsonClassMap.RegisterClassMap<NotionUploadProjectTaskWorkflowStep>(cm => { cm.AutoMap(); });
            }
        }
    }
}