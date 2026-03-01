using MongoDB.Bson.Serialization;
using Rocket.Diagnostics.Domain;
using Rocket.Interfaces;

namespace Rocket.Diagnostics.Infrastructure
{
    public class DiagnosticsBsonMapper : IBsonMapper
    {
        public void MapApplicableBsonTypes()
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(HelloWorldTextExecutionStep)))
            {
                BsonClassMap.RegisterClassMap<HelloWorldTextExecutionStep>(cm =>
                {
                    cm.AutoMap();
                });
            }
            
            if (!BsonClassMap.IsClassMapRegistered(typeof(HelloWorldProjectExecutionStep)))
            {
                BsonClassMap.RegisterClassMap<HelloWorldProjectExecutionStep>(cm =>
                {
                    cm.AutoMap();
                });
            }
            
            if (!BsonClassMap.IsClassMapRegistered(typeof(HelloWorldTextWorkflowStep)))
            {
                BsonClassMap.RegisterClassMap<HelloWorldTextWorkflowStep>(cm =>
                {
                    cm.AutoMap();
                });
            }
            
            if (!BsonClassMap.IsClassMapRegistered(typeof(HelloWorldProjectWorkflowStep)))
            {
                BsonClassMap.RegisterClassMap<HelloWorldProjectWorkflowStep>(cm =>
                {
                    cm.AutoMap();
                });
            }
        }
    }
}