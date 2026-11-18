using MongoDB.Bson.Serialization;
using Rocket.Interfaces;
using Rocket.Replicate.Domain.Models.DataLabTo.Markdown;
using Rocket.Replicate.Domain.Models.DataLabTo.Project;
using Rocket.Replicate.Domain.Models.DataLabTo.Text;

namespace Rocket.Replicate.Infrastructure.Models.DataLabTo
{
    public class DataLabToBsonMapper : IBsonMapper
    {
        public void MapApplicableBsonTypes()
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(DataLabToExtractTextExecutionStep)))
            {
                BsonClassMap.RegisterClassMap<DataLabToExtractTextExecutionStep>(cm =>
                {
                    cm.AutoMap();
                });
            }
            
            if (!BsonClassMap.IsClassMapRegistered(typeof(DataLabToExtractProjectExecutionStep)))
            {
                BsonClassMap.RegisterClassMap<DataLabToExtractProjectExecutionStep>(cm =>
                {
                    cm.AutoMap();
                });
            }
            
            if (!BsonClassMap.IsClassMapRegistered(typeof(DataLabToExtractMarkdownExecutionStep)))
            {
                BsonClassMap.RegisterClassMap<DataLabToExtractMarkdownExecutionStep>(cm =>
                {
                    cm.AutoMap();
                });
            }
            
            if (!BsonClassMap.IsClassMapRegistered(typeof(DataLabToExtractTextWorkflowStep)))
            {
                BsonClassMap.RegisterClassMap<DataLabToExtractTextWorkflowStep>(cm =>
                {
                    cm.AutoMap();
                });
            }
            
            if (!BsonClassMap.IsClassMapRegistered(typeof(DataLabToExtractProjectWorkflowStep)))
            {
                BsonClassMap.RegisterClassMap<DataLabToExtractProjectWorkflowStep>(cm =>
                {
                    cm.AutoMap();
                });
            }
            
            if (!BsonClassMap.IsClassMapRegistered(typeof(DataLabToExtractMarkdownWorkflowStep)))
            {
                BsonClassMap.RegisterClassMap<DataLabToExtractMarkdownWorkflowStep>(cm =>
                {
                    cm.AutoMap();
                });
            }
        }
    }
}