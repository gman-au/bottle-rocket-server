using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Rocket.Domain.Workflows;
using Rocket.Interfaces;

namespace Rocket.Infrastructure.Db.Mongo
{
    public class MongoDbWorkflowRepository(
        ILogger<MongoDbWorkflowRepository> logger,
        IMongoDbClient mongoDbClient
    ) : MongoDbRepositoryBase<Workflow>(mongoDbClient, logger), IWorkflowRepository
    {
        protected override string CollectionName => MongoConstants.WorkflowsCollection;

        public async Task<Workflow> InsertWorkflowAsync(
            Workflow workflow,
            CancellationToken cancellationToken) =>
            await
                InsertRecordAsync(workflow, cancellationToken);

        public async Task<(IEnumerable<Workflow> records, long totalRecordCount)> FetchWorkflowsAsync(
            string userId,
            int? startIndex,
            int? recordCount,
            CancellationToken cancellationToken
        ) => await
            FetchAllPagedAndFilteredRecordsAsync(
                startIndex,
                recordCount,
                Builders<Workflow>
                    .Filter
                    .Eq(
                        u => u.UserId,
                        userId
                    ),
                o => o.CreatedAt,
                cancellationToken
            );

        public async Task<Workflow> GetWorkflowByIdAsync(
            string userId,
            string id,
            CancellationToken cancellationToken
        ) =>
            await
                FetchFirstFilteredRecordAsync(
                    Builders<Workflow>
                        .Filter
                        .Eq(
                            o => o.UserId,
                            userId
                        ) &
                    Builders<Workflow>
                        .Filter
                        .Eq(
                            o => o.Id,
                            id
                        ),
                    cancellationToken
                );

        public async Task<Workflow> GetWorkflowByNameAsync(
            string userId,
            string name,
            CancellationToken cancellationToken
        ) =>
            await
                FetchFirstFilteredRecordAsync(
                    Builders<Workflow>
                        .Filter
                        .Eq(
                            o => o.UserId,
                            userId
                        ) &
                    Builders<Workflow>
                        .Filter
                        .Eq(
                            o => o.Name,
                            name
                        ),
                    cancellationToken
                );

        public async Task<bool> DeleteWorkflowAsync(
            string userId,
            string id,
            CancellationToken cancellationToken) =>
            await
                DeleteFirstFilteredRecordAsync(
                    Builders<Workflow>
                        .Filter
                        .Eq(
                            o => o.UserId,
                            userId
                        ) &
                    Builders<Workflow>
                        .Filter
                        .Eq(
                            o => o.Id,
                            id
                        ),
                    cancellationToken
                );

        public async Task UpdateWorkflowFieldAsync<TField>(
            string workflowId,
            string userId,
            Expression<Func<Workflow, TField>> setter,
            TField value,
            CancellationToken cancellationToken
        ) =>
            await
                ApplyUpdateToFilteredRecordFieldAsync(
                    setter,
                    value,
                    Builders<Workflow>
                        .Filter
                        .Eq(
                            u => u.UserId,
                            userId
                        ) &
                    Builders<Workflow>
                        .Filter
                        .Eq(
                            o => o.Id,
                            workflowId
                        ),
                    cancellationToken
                );

        public async Task<bool> WorkflowExistsForUserAsync(
            string userId,
            string workflowName,
            CancellationToken cancellationToken
        )
        {
            var result =
                await
                    GetWorkflowByNameAsync(
                        userId,
                        workflowName,
                        cancellationToken
                    ) != null;

            return result;
        }
    }
}