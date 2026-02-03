using MongoDB.Bson.Serialization;
using Rocket.Gcp.Domain;
using Rocket.Interfaces;

namespace Rocket.Gcp.Infrastructure
{
    public class GcpBsonMapper : IBsonMapper
    {
        public void MapApplicableBsonTypes()
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(GcpConnector)))
            {
                BsonClassMap.RegisterClassMap<GcpConnector>(cm =>
                {
                    cm.AutoMap();
                });
            }
            
            if (!BsonClassMap.IsClassMapRegistered(typeof(GcpExtractExecutionStep)))
            {
                BsonClassMap.RegisterClassMap<GcpExtractExecutionStep>(cm =>
                {
                    cm.AutoMap();
                });
            }
            
            if (!BsonClassMap.IsClassMapRegistered(typeof(GcpUploadExecutionStep)))
            {
                BsonClassMap.RegisterClassMap<GcpUploadExecutionStep>(cm =>
                {
                    cm.AutoMap();
                });
            }
            
            if (!BsonClassMap.IsClassMapRegistered(typeof(GcpExtractExecutionStep)))
            {
                BsonClassMap.RegisterClassMap<GcpExtractExecutionStep>(cm =>
                {
                    cm.AutoMap();
                });
            }
            
            if (!BsonClassMap.IsClassMapRegistered(typeof(GcpUploadWorkflowStep)))
            {
                BsonClassMap.RegisterClassMap<GcpUploadWorkflowStep>(cm =>
                {
                    cm.AutoMap();
                });
            }
        }
    }
}