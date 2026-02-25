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
            
            if (!BsonClassMap.IsClassMapRegistered(typeof(OllamaExtractTextExecutionStep)))
            {
                BsonClassMap.RegisterClassMap<OllamaExtractTextExecutionStep>(cm =>
                {
                    cm.AutoMap();
                });
            }
            
            if (!BsonClassMap.IsClassMapRegistered(typeof(OllamaExtractTextWorkflowStep)))
            {
                BsonClassMap.RegisterClassMap<OllamaExtractTextWorkflowStep>(cm =>
                {
                    cm.AutoMap();
                });
            }
        }
    }
}