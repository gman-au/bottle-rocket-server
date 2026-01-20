using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Rocket.Domain.Enum;
using Rocket.Domain.Exceptions;
using Rocket.Domain.Executions;
using Rocket.Infrastructure.Db.Mongo.Extensions;
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
                o => o.CreatedAt,
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

        public async Task<Execution> UpdateExecutionStepAsync<TExecutionStep>(
            string executionStepId,
            string executionId,
            string userId,
            TExecutionStep updatedExecutionStep,
            CancellationToken cancellationToken
        ) where TExecutionStep : BaseExecutionStep
        {
            var filter =
                Builders<Execution>
                    .Filter
                    .Eq(
                        u => u.UserId,
                        userId
                    );

            filter &=
                Builders<Execution>
                    .Filter
                    .Eq(
                        o => o.Id,
                        executionId
                    );

            var execution =
                await
                    GetMongoCollection()
                        .Find(filter)
                        .FirstOrDefaultAsync(cancellationToken);

            if (execution == null)
                throw new RocketException(
                    $"Could not find execution record with id [{executionId}]",
                    ApiStatusCodeEnum.UnknownOrInaccessibleRecord
                );

            if (!BaseExecutionStepEx.UpdateStepById(
                    execution.Steps,
                    executionStepId,
                    updatedExecutionStep,
                    out var modifiedSteps
                ))
                throw new RocketException(
                    $"Could not find execution step record with id [{executionStepId}]",
                    ApiStatusCodeEnum.UnknownOrInaccessibleRecord
                );

            execution.Steps = modifiedSteps;

            await
                GetMongoCollection()
                    .ReplaceOneAsync(
                        filter,
                        execution,
                        cancellationToken: cancellationToken
                    );

            return execution;
        }
    }
}