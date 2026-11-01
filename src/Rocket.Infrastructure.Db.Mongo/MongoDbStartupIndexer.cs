using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Rocket.Domain;
using Rocket.Interfaces;

namespace Rocket.Infrastructure.Db.Mongo
{
    public class MongoDbStartupIndexer(
        ILogger<MongoDbStartupIndexer> logger,
        IMongoDbClient mongoDbClient
    ) : IDatabasePreRunAction
    {
        public async Task RunAsync(
            CancellationToken cancellationToken
        )
        {
            logger
                .LogInformation("Running MongoDB startup indexer...");
            
            var mongoDatabase =
                mongoDbClient
                    .GetDatabase();

            var tokenCollection =
                mongoDatabase
                    .GetCollection<TokenSession>(MongoConstants.TokenSessionsCollection);

            await
                tokenCollection
                    .Indexes
                    .CreateManyAsync(
                        [
                            new CreateIndexModel<TokenSession>(
                                Builders<TokenSession>
                                    .IndexKeys
                                    .Ascending(t => t.Hash),
                                new CreateIndexOptions
                                {
                                    Unique = true
                                }
                            ),
                            new CreateIndexModel<TokenSession>(
                                Builders<TokenSession>
                                    .IndexKeys
                                    .Ascending(t => t.PreviousHash),
                                new CreateIndexOptions
                                {
                                    Sparse = true
                                }
                            ),
                            new CreateIndexModel<TokenSession>(
                                Builders<TokenSession>
                                    .IndexKeys
                                    .Ascending(t => t.UserId)
                                    .Ascending(t => t.IsActive)
                            ),
                            // Mongo deletes expired sessions automatically
                            new CreateIndexModel<TokenSession>(
                                Builders<TokenSession>
                                    .IndexKeys
                                    .Ascending(t => t.ExpiresAt),
                                new CreateIndexOptions
                                {
                                    ExpireAfter = TimeSpan.Zero
                                }
                            )
                        ],
                        cancellationToken
                    );
        }
    }
}