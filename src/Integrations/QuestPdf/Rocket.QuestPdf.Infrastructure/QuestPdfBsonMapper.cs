using MongoDB.Bson.Serialization;
using Rocket.Interfaces;
using Rocket.QuestPdf.Domain;

namespace Rocket.QuestPdf.Infrastructure
{
    public class QuestPdfBsonMapper : IBsonMapper
    {
        public void MapApplicableBsonTypes()
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(ConvertToPdfExecutionStep)))
            {
                BsonClassMap.RegisterClassMap<ConvertToPdfExecutionStep>(cm =>
                {
                    cm.AutoMap();
                });
            }
            
            if (!BsonClassMap.IsClassMapRegistered(typeof(ConvertToPdfWorkflowStep)))
            {
                BsonClassMap.RegisterClassMap<ConvertToPdfWorkflowStep>(cm =>
                {
                    cm.AutoMap();
                });
            }
        }
    }
}