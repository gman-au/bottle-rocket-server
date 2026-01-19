using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Rocket.Domain.Executions;
using Rocket.Interfaces;

namespace Rocket.Infrastructure.Db.Mongo
{
    public class MongoDbExecutionRepository(
        ILogger<MongoDbExecutionRepository> logger,
        IMongoDbClient mongoDbClient
    ) : MongoDbRepositoryBase<Execution>(
        mongoDbClient,
        logger
    ), IExecutionRepository
    {
        protected override string CollectionName => MongoConstants.ExecutionsCollection;

        public async Task<(IEnumerable<Execution> records, long totalRecordCount)> FetchExecutionsAsync(
            string userId,
            int startIndex,
            int recordCount,
            CancellationToken cancellationToken
        ) => await
            FetchAllPagedAndFilteredRecordsAsync(
                startIndex,
                recordCount,
                Builders<Execution>
                    .Filter
                    .Eq(
                        u => u.UserId,
                        userId
                    ),
                o => o.RunDate,
                cancellationToken
            );

        public async Task<bool> DeleteExecutionAsync(
            string userId,
            string id,
            CancellationToken cancellationToken
        ) =>
            await
                DeleteFirstFilteredRecordAsync(
                    Builders<Execution>
                        .Filter
                        .Eq(
                            o => o.UserId,
                            userId
                        ) &
                    Builders<Execution>
                        .Filter
                        .Eq(
                            o => o.Id,
                            id
                        ),
                    cancellationToken
                );

        public async Task<Execution> InsertExecutionAsync(
            Execution execution,
            CancellationToken cancellationToken
        ) =>
            await
                InsertRecordAsync(
                    execution,
                    cancellationToken
                );

        public async Task<Execution> GetExecutionByIdAsync(
            string userId,
            string id,
            CancellationToken cancellationToken
        ) =>
            await
                FetchFirstFilteredRecordAsync(
                    Builders<Execution>
                        .Filter
                        .Eq(
                            o => o.UserId,
                            userId
                        ) &
                    Builders<Execution>
                        .Filter
                        .Eq(
                            o => o.Id,
                            id
                        ),
                    cancellationToken
                );
    }
}