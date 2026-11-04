using MongoDB.Bson.Serialization;
using Rocket.Infrastructure.Encryption;
using Rocket.Interfaces;
using Rocket.Microsofts.Domain;

namespace Rocket.Microsofts.Infrastructure
{
    public class MicrosoftBsonMapper(
        EncryptedStringSerializer enc
    ) : IBsonMapper
    {
        public void MapApplicableBsonTypes()
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(MicrosoftConnector)))
            {
                BsonClassMap.RegisterClassMap<MicrosoftConnector>(cm =>
                {
                    cm.AutoMap();
                    cm.GetMemberMap(o => o.RefreshToken)
                        .SetSerializer(enc);
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
            
            if (!BsonClassMap.IsClassMapRegistered(typeof(OneNoteUploadExecutionStep)))
            {
                BsonClassMap.RegisterClassMap<OneNoteUploadExecutionStep>(cm =>
                {
                    cm.AutoMap();
                });
            }
            
            if (!BsonClassMap.IsClassMapRegistered(typeof(OneNoteUploadWorkflowStep)))
            {
                BsonClassMap.RegisterClassMap<OneNoteUploadWorkflowStep>(cm =>
                {
                    cm.AutoMap();
                });
            }
        }
    }
}