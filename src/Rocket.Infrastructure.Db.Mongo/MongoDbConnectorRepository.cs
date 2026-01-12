using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Rocket.Domain;
using Rocket.Domain.Connectors;
using Rocket.Interfaces;

namespace Rocket.Infrastructure.Db.Mongo
{
    public class MongoDbConnectorRepository(
        ILogger<MongoDbConnectorRepository> logger,
        IMongoDbClient mongoDbClient
    ) : IConnectorRepository
    {
        public async Task<BaseConnector> SaveConnectorAsync(BaseConnector baseConnector, CancellationToken cancellationToken)
        {
            try
            {
                var mongoDatabase =
                    mongoDbClient
                        .GetDatabase();

                var connectorCollection =
                    mongoDatabase
                        .GetCollection<BaseConnector>(MongoConstants.ConnectorsCollection);

                await
                    connectorCollection
                        .InsertOneAsync(
                            baseConnector,
                            new InsertOneOptions(),
                            cancellationToken
                        );

                return baseConnector;
            }
            catch (Exception ex)
            {
                logger
                    .LogError(
                        "There was an error saving the connector: {error}",
                        ex.Message
                    );

                throw;
            }
        }

        public async Task<(IEnumerable<BaseConnector> records, long totalRecordCount)> FetchConnectorsAsync(
            string userId,
            int startIndex,
            int recordCount,
            CancellationToken cancellationToken
        )
        {
            try
            {
                var mongoDatabase =
                    mongoDbClient
                        .GetDatabase();

                var connectorsCollection =
                    mongoDatabase
                        .GetCollection<BaseConnector>(MongoConstants.ConnectorsCollection);

                var filter =
                    Builders<BaseConnector>
                        .Filter
                        .Eq(
                            u => u.UserId,
                            userId
                        );

                var totalRecordCount =
                    await
                        connectorsCollection
                            .Find(filter)
                            .CountDocumentsAsync(cancellationToken: cancellationToken);

                var records =
                    await
                        connectorsCollection
                            .Find(filter)
                            .SortByDescending(x => x.CreatedAt)
                            .Skip(startIndex)
                            .Limit(recordCount)
                            .ToListAsync(cancellationToken: cancellationToken);

                return (records, totalRecordCount);
            }
            catch (Exception ex)
            {
                logger
                    .LogError(
                        "There was an fetching connectors: {error}",
                        ex.Message
                    );

                throw;
            }
        }

        public async Task<BaseConnector> FetchConnectorAsync(
            string userId,
            string id,
            CancellationToken cancellationToken
        )
        {
            try
            {
                var mongoDatabase =
                    mongoDbClient
                        .GetDatabase();

                var connectorCollection =
                    mongoDatabase
                        .GetCollection<BaseConnector>(MongoConstants.ConnectorsCollection);

                var filter =
                    Builders<BaseConnector>
                        .Filter
                        .Eq(
                            o => o.UserId,
                            userId
                        );

                filter &=
                    Builders<BaseConnector>
                        .Filter
                        .Eq(
                            o => o.Id,
                            id
                        );

                var record =
                    await
                        connectorCollection
                            .Find(filter)
                            .FirstOrDefaultAsync(cancellationToken: cancellationToken);

                return record;
            }
            catch (Exception ex)
            {
                logger
                    .LogError(
                        "There was an fetching connector: {error}",
                        ex.Message
                    );

                throw;
            }
        }

        public async Task<bool> DeleteConnectorAsync(string userId, string id, CancellationToken cancellationToken)
        {
            var mongoDatabase =
                mongoDbClient
                    .GetDatabase();

            var connectorCollection =
                mongoDatabase
                    .GetCollection<BaseConnector>(MongoConstants.ConnectorsCollection);

            var filter =
                Builders<BaseConnector>
                    .Filter
                    .Eq(
                        o => o.UserId,
                        userId
                    );

            filter &=
                Builders<BaseConnector>
                    .Filter
                    .Eq(
                        o => o.Id,
                        id
                    );

            var record =
                await
                    connectorCollection
                        .DeleteOneAsync(
                            filter,
                            cancellationToken
                        );

            return
                record
                    .DeletedCount > 0;
        }
    }
}