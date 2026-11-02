using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using Rocket.Interfaces;

namespace Rocket.Infrastructure.Db.Mongo
{
    public class MongoDbPing(
        ILogger<MongoDbPing> logger,
        IMongoDbClient mongoDbClient
    ) : IAppHealthCheck
    {
        public async Task PreRunAsync()
        {
            try
            {
                var mongoDatabase =
                    mongoDbClient
                        .GetDatabase();

                logger
                    .LogInformation(
                        "Attempting to connect to MongoDB database [{DatabaseNamespace}] at {DatabaseHost}...",
                        mongoDatabase.DatabaseNamespace,
                        mongoDbClient.GetClient().Settings.Server.Host
                    );

                await
                    mongoDatabase
                        .RunCommandAsync<BsonDocument>(
                            new BsonDocument(
                                "ping",
                                1
                            )
                        );
                
                logger
                    .LogInformation(
                        "Database [{DatabaseNamespace}] connected at {DatabaseHost}",
                        mongoDatabase.DatabaseNamespace,
                        mongoDbClient.GetClient().Settings.Server.Host
                    );
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to connect to MongoDB", ex);
            }
        }
    }
}