using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Rocket.Domain.Core;
using Rocket.Interfaces;

namespace Rocket.Infrastructure.Db.Mongo
{
    public class MongoDbConnectorRepository(
        ILogger<MongoDbConnectorRepository> logger,
        IMongoDbClient mongoDbClient
    ) : MongoDbRepositoryBase<CoreConnector>(
        mongoDbClient,
        logger
    ), IConnectorRepository
    {
        protected override string CollectionName => MongoConstants.ConnectorsCollection;

        public async Task<CoreConnector> InsertConnectorAsync(
            CoreConnector coreConnector,
            CancellationToken cancellationToken
        ) =>
            await
                InsertRecordAsync(
                    coreConnector,
                    cancellationToken
                );

        public async Task<(IEnumerable<CoreConnector> records, long totalRecordCount)> FetchConnectorsAsync(
            string userId,
            int startIndex,
            int recordCount,
            CancellationToken cancellationToken
        ) =>
            await
                FetchAllPagedAndFilteredRecordsAsync(
                    startIndex,
                    recordCount,
                    Builders<CoreConnector>
                        .Filter
                        .Eq(
                            u => u.UserId,
                            userId
                        ),
                    o => o.CreatedAt,
                    cancellationToken
                );

        public async Task<(IEnumerable<CoreConnector> records, long totalRecordCount)> FetchConnectorsByCodeAndUserAsync(
            string userId,
            int? startIndex,
            int? recordCount,
            string code,
            CancellationToken cancellationToken
        ) =>
            await
                FetchAllPagedAndFilteredRecordsAsync(
                    startIndex,
                    recordCount,
                    Builders<CoreConnector>
                        .Filter
                        .Eq(
                            u => u.UserId,
                            userId
                        ) &
                    Builders<CoreConnector>
                        .Filter
                        .Eq(
                            o => o.ConnectorCode,
                            code
                        ),
                    o => o.CreatedAt,
                    cancellationToken
                );

        public async Task<T> GetConnectorByIdAsync<T>(
            string userId,
            string id,
            CancellationToken cancellationToken
        ) where T : CoreConnector =>
            await
                FetchFirstFilteredRecordAsync(
                    Builders<CoreConnector>
                        .Filter
                        .Eq(
                            o => o.UserId,
                            userId
                        ) &
                    Builders<CoreConnector>
                        .Filter
                        .Eq(
                            o => o.Id,
                            id
                        ),
                    cancellationToken
                ) as T;

        public async Task<T> GetConnectorByNameAsync<T>(
            string userId,
            string name,
            CancellationToken cancellationToken
        ) where T : CoreConnector =>
            await
                FetchFirstFilteredRecordAsync(
                    Builders<CoreConnector>
                        .Filter
                        .Eq(
                            o => o.UserId,
                            userId
                        ) &
                    Builders<CoreConnector>
                        .Filter
                        .Eq(
                            o => o.ConnectorName,
                            name
                        ),
                    cancellationToken
                ) as T;

        public async Task<bool> DeleteConnectorAsync(
            string userId,
            string id,
            CancellationToken cancellationToken
        ) =>
            await
                DeleteFirstFilteredRecordAsync(
                    Builders<CoreConnector>
                        .Filter
                        .Eq(
                            o => o.UserId,
                            userId
                        ) &
                    Builders<CoreConnector>
                        .Filter
                        .Eq(
                            o => o.Id,
                            id
                        ),
                    cancellationToken
                );

        public async Task UpdateConnectorFieldAsync<TConnector, TField>(
            string connectorId,
            string userId,
            Expression<Func<TConnector, TField>> setter,
            TField value,
            CancellationToken cancellationToken
        ) where TConnector : CoreConnector
        {
            try
            {
                var filter =
                    Builders<TConnector>
                        .Filter
                        .Eq(
                            u => u.UserId,
                            userId
                        );

                filter &=
                    Builders<TConnector>
                        .Filter
                        .Eq(
                            o => o.Id,
                            connectorId
                        );

                var update =
                    Builders<TConnector>
                        .Update
                        .Set(
                            setter,
                            value
                        );

                await
                    UpdateConnectorAsync
                    (
                        filter,
                        update,
                        cancellationToken
                    );
            }
            catch (Exception ex)
            {
                logger
                    .LogError(
                        "Error updating updating the field for connector {id}, user {userId}: {error}",
                        connectorId,
                        userId,
                        ex.Message
                    );
                throw;
            }
        }

        public async Task<bool> ConnectorExistsForUserAsync(
            string userId,
            string connectorName,
            CancellationToken cancellationToken
        )
        {
            var result =
                await
                    GetConnectorByNameAsync<CoreConnector>(
                        userId,
                        connectorName,
                        cancellationToken
                    ) != null;

            return result;
        }

        private async Task UpdateConnectorAsync<T>(
            FilterDefinition<T> filter,
            UpdateDefinition<T> update,
            CancellationToken cancellationToken
        ) where T : CoreConnector
        {
            await
                GetMongoCollection<T>()
                    .UpdateOneAsync(
                        filter,
                        update,
                        new UpdateOptions(),
                        cancellationToken
                    );
        }
    }
}