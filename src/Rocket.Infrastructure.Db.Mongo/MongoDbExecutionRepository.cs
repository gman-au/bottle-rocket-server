using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Rocket.Domain.Core;
using Rocket.Domain.Core.Enum;
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
            int? startIndex,
            int? recordCount,
            string scanId,
            string workflowId,
            CancellationToken cancellationToken
        )
        {
            var filter =
                Builders<Execution>
                    .Filter
                    .Eq(
                        u => u.UserId,
                        userId
                    );

            if (!string.IsNullOrWhiteSpace(scanId))
            {
                filter &=
                    Builders<Execution>
                        .Filter
                        .Eq(
                            o => o.ScanId,
                            scanId
                        );
            }

            if (!string.IsNullOrWhiteSpace(workflowId))
            {
                filter &=
                    Builders<Execution>
                        .Filter
                        .Eq(
                            o => o.WorkflowId,
                            workflowId
                        );
            }

            return
                await
                    FetchAllPagedAndFilteredRecordsAsync(
                        startIndex,
                        recordCount,
                        filter,
                        o => o.CreatedAt,
                        cancellationToken
                    );
        }

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
        ) where TExecutionStep : CoreExecutionStep
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

            if (!CoreExecutionStepEx.UpdateStepById(
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
        
        public async Task UpdateExecutionFieldAsync<TField>(
            string executionId,
            string userId,
            Expression<Func<Execution, TField>> setter,
            TField value,
            CancellationToken cancellationToken
        ) =>
            await
                ApplyUpdateToFilteredRecordFieldAsync(
                    setter,
                    value,
                    Builders<Execution>
                        .Filter
                        .Eq(
                            u => u.UserId,
                            userId
                        ) &
                    Builders<Execution>
                        .Filter
                        .Eq(
                            o => o.Id,
                            executionId
                        ),
                    cancellationToken
                );
    }
}