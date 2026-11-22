using MongoDB.Bson.Serialization;
using Rocket.Infrastructure.Encryption;
using Rocket.Interfaces;
using Rocket.OpenAi.Domain;
using Rocket.OpenAi.Domain.Project;
using Rocket.OpenAi.Domain.Retitle;
using Rocket.OpenAi.Domain.Text;

namespace Rocket.OpenAi.Infrastructure
{
    public class OpenAiBsonMapper(
        EncryptedStringSerializer enc
    ) : IBsonMapper
    {
        public void MapApplicableBsonTypes()
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(OpenAiConnector)))
            {
                BsonClassMap.RegisterClassMap<OpenAiConnector>(cm =>
                    {
                        cm.AutoMap();
                        cm.GetMemberMap(o => o.ApiKey)
                            .SetSerializer(enc);
                    }
                );
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(OpenAiExtractTextExecutionStep)))
            {
                BsonClassMap.RegisterClassMap<OpenAiExtractTextExecutionStep>(cm => { cm.AutoMap(); });
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(OpenAiExtractProjectExecutionStep)))
            {
                BsonClassMap.RegisterClassMap<OpenAiExtractProjectExecutionStep>(cm => { cm.AutoMap(); });
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(OpenAiRetitleFileExecutionStep)))
            {
                BsonClassMap.RegisterClassMap<OpenAiRetitleFileExecutionStep>(cm => { cm.AutoMap(); });
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(OpenAiExtractTextWorkflowStep)))
            {
                BsonClassMap.RegisterClassMap<OpenAiExtractTextWorkflowStep>(cm => { cm.AutoMap(); });
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(OpenAiExtractProjectWorkflowStep)))
            {
                BsonClassMap.RegisterClassMap<OpenAiExtractProjectWorkflowStep>(cm => { cm.AutoMap(); });
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(OpenAiRetitleFileWorkflowStep)))
            {
                BsonClassMap.RegisterClassMap<OpenAiRetitleFileWorkflowStep>(cm => { cm.AutoMap(); });
            }
        }
    }
}