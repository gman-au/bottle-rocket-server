using MongoDB.Bson.Serialization;
using Rocket.Domain;
using Rocket.Domain.Connectors;
using Rocket.Dropbox.Domain;
using Rocket.MaxOcr.Domain;

namespace Rocket.Api.Host.Bson
{
    internal static class MongoBsonClassMapping
    {
        public static void RegisterBaseConnectors()
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(BaseConnector)))
            {
                BsonClassMap.RegisterClassMap<BaseConnector>(cm =>
                {
                    cm.AutoMap();
                    cm.SetIsRootClass(true);
                });
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(DropboxConnector)))
            {
                BsonClassMap.RegisterClassMap<DropboxConnector>(cm =>
                {
                    cm.AutoMap();
                });
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(MaxOcrConnector)))
            {
                BsonClassMap.RegisterClassMap<MaxOcrConnector>(cm =>
                {
                    cm.AutoMap();
                });
            }
        }
    }
}