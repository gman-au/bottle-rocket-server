using MongoDB.Bson.Serialization;
using Rocket.Google.Domain;
using Rocket.Infrastructure.Encryption;
using Rocket.Interfaces;

namespace Rocket.Google.Infrastructure
{
    public class GoogleBsonMapper(
        EncryptedStringSerializer enc
    ) : IBsonMapper
    {
        public void MapApplicableBsonTypes()
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(GoogleConnector)))
            {
                BsonClassMap.RegisterClassMap<GoogleConnector>(cm =>
                {
                    cm.AutoMap();
                    cm.GetMemberMap(o => o.AccessToken)
                        .SetSerializer(enc);
                    cm.GetMemberMap(o => o.RefreshToken)
                        .SetSerializer(enc);
                });
                
                BsonClassMap.RegisterClassMap<GoogleInstalledCredential>(cm =>
                {
                    cm.AutoMap();
                    cm.GetMemberMap(o => o.ClientId)
                        .SetSerializer(enc);
                    cm.GetMemberMap(o => o.ClientSecret)
                        .SetSerializer(enc);
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