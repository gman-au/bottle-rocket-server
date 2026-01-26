using MongoDB.Bson.Serialization;
using Rocket.Interfaces;
using Rocket.Ollama.Domain;

namespace Rocket.Ollama.Infrastructure
{
    public class OllamaBsonMapper : IBsonMapper
    {
        public void MapApplicableBsonTypes()
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(OllamaConnector)))
            {
                BsonClassMap.RegisterClassMap<OllamaConnector>(cm =>
                {
                    cm.AutoMap();
                });
            }
            
            if (!BsonClassMap.IsClassMapRegistered(typeof(OllamaExtractExecutionStep)))
            {
                BsonClassMap.RegisterClassMap<OllamaExtractExecutionStep>(cm =>
                {
                    cm.AutoMap();
                });
            }
            
            if (!BsonClassMap.IsClassMapRegistered(typeof(OllamaExtractWorkflowStep)))
            {
                BsonClassMap.RegisterClassMap<OllamaExtractWorkflowStep>(cm =>
                {
                    cm.AutoMap();
                });
            }
        }
    }
}