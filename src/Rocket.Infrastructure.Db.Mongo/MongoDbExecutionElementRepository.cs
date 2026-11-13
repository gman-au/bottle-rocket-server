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
    public class MongoDbExecutionElementRepository(
        ILogger<MongoDbExecutionElementRepository> logger,
        IMongoDbClient mongoDbClient
    ) : MongoDbRepositoryBase<Execution>(
        mongoDbClient,
        logger
    ), IExecutionElementRepository
    {
        protected override string CollectionName => MongoConstants.ExecutionsCollection;

        public async Task<ExecutionElement> GetExecutionElementByScanIdAsync(
            string userId,
            string executionId,
            string scanId,
            CancellationToken cancellationToken
        )
        {
            var execution =
                await
                    GetExecutionByIdAsync(
                        executionId,
                        userId,
                        cancellationToken
                    );

            var element =
                execution
                    .Elements
                    .FindElementByScanId(
                        scanId
                    );

            if (element == null)
                throw new RocketException(
                    $"Could not find element in execution ID [{executionId}] with scan ID [{scanId}]",
                    ApiStatusCodeEnum.UnknownOrInaccessibleRecord
                );

            return element;
        }

        public async Task<Execution> UpdateExecutionElementAsync(
            string executionElementId,
            string executionId,
            string userId,
            ExecutionElement updatedElement,
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

            if (!BaseExecutionElementEx.UpdateElementById(
                    execution.Elements,
                    executionElementId,
                    updatedElement,
                    out var modifiedElements
                ))
                throw new RocketException(
                    $"Could not find execution element record with id [{executionElementId}]",
                    ApiStatusCodeEnum.UnknownOrInaccessibleRecord
                );

            execution.Elements = modifiedElements;

            await
                GetMongoCollection()
                    .ReplaceOneAsync(
                        filter,
                        execution,
                        cancellationToken: cancellationToken
                    );

            return execution;
        }

        private async Task<Execution> GetExecutionByIdAsync(
            string executionId,
            string userId,
            CancellationToken cancellationToken
        )
        {
            var filter =
                BuildExecutionFilterByIdAndUserId(
                    executionId,
                    userId
                );

            var execution =
                await
                    GetMongoCollection()
                        .Find(filter)
                        .FirstOrDefaultAsync(cancellationToken);

            if (execution == null)
                throw new RocketException(
                    $"Could not find workflow record with id [{executionId}]",
                    ApiStatusCodeEnum.UnknownOrInaccessibleRecord
                );

            return execution;
        }

        private static FilterDefinition<Execution> BuildExecutionFilterByIdAndUserId(
            string executionId,
            string userId
        ) =>
            Builders<Execution>
                .Filter
                .Eq(
                    w => w.Id,
                    executionId
                ) &
            Builders<Execution>
                .Filter
                .Eq(
                    w => w.UserId,
                    userId
                );
    }
}