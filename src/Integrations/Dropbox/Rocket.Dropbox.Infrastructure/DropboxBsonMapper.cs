using MongoDB.Bson.Serialization;
using Rocket.Dropbox.Domain;
using Rocket.Infrastructure.Encryption;
using Rocket.Interfaces;

namespace Rocket.Dropbox.Infrastructure
{
    public class DropboxBsonMapper(
        EncryptedStringSerializer enc
    ) : IBsonMapper
    {
        public void MapApplicableBsonTypes()
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(DropboxConnector)))
            {
                BsonClassMap.RegisterClassMap<DropboxConnector>(cm =>
                    {
                        cm.AutoMap();
                        cm.GetMemberMap(o => o.AppKey)
                            .SetSerializer(enc);
                        cm.GetMemberMap(o => o.AppSecret)
                            .SetSerializer(enc);
                        cm.GetMemberMap(o => o.RefreshToken)
                            .SetSerializer(enc);
                    }
                );
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(DropboxUploadExecutionStep)))
            {
                BsonClassMap.RegisterClassMap<DropboxUploadExecutionStep>(cm => { cm.AutoMap(); });
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(DropboxUploadWorkflowStep)))
            {
                BsonClassMap.RegisterClassMap<DropboxUploadWorkflowStep>(cm => { cm.AutoMap(); });
            }
        }
    }
}