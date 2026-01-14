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
    ) : IWorkflowRepository
    {
        public async Task<Workflow> SaveWorkflowAsync(
            Workflow workflow,
            CancellationToken cancellationToken
        )
        {
            try
            {
                var mongoDatabase =
                    mongoDbClient
                        .GetDatabase();

                var workflowCollection =
                    mongoDatabase
                        .GetCollection<Workflow>(MongoConstants.WorkflowsCollection);

                await
                    workflowCollection
                        .InsertOneAsync(
                            workflow,
                            new InsertOneOptions(),
                            cancellationToken
                        );

                return workflow;
            }
            catch (Exception ex)
            {
                logger
                    .LogError(
                        "There was an error saving the workflow: {error}",
                        ex.Message
                    );

                throw;
            }
        }

        public async Task<(IEnumerable<Workflow> records, long totalRecordCount)> FetchWorkflowsAsync(
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

                var workflowsCollection =
                    mongoDatabase
                        .GetCollection<Workflow>(MongoConstants.WorkflowsCollection);

                var filter =
                    Builders<Workflow>
                        .Filter
                        .Eq(
                            u => u.UserId,
                            userId
                        );

                var totalRecordCount =
                    await
                        workflowsCollection
                            .Find(filter)
                            .CountDocumentsAsync(cancellationToken: cancellationToken);

                var records =
                    await
                        workflowsCollection
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
                        "There was an fetching workflows: {error}",
                        ex.Message
                    );

                throw;
            }
        }

        public async Task<Workflow> FetchWorkflowByIdAsync(
            string userId,
            string id,
            CancellationToken cancellationToken
        )
        {
            var filter =
                Builders<Workflow>
                    .Filter
                    .Eq(
                        o => o.UserId,
                        userId
                    );

            filter &=
                Builders<Workflow>
                    .Filter
                    .Eq(
                        o => o.Id,
                        id
                    );

            return
                await
                    FetchWorkflowByFilterAsync(
                        filter,
                        cancellationToken
                    );
        }

        public async Task<Workflow> FetchWorkflowByNameAsync(
            string userId,
            string name,
            CancellationToken cancellationToken
        )
        {
            var filter =
                Builders<Workflow>
                    .Filter
                    .Eq(
                        o => o.UserId,
                        userId
                    );

            filter &=
                Builders<Workflow>
                    .Filter
                    .Eq(
                        o => o.Name,
                        name
                    );

            return
                await
                    FetchWorkflowByFilterAsync(
                        filter,
                        cancellationToken
                    );
        }

        public async Task<bool> DeleteWorkflowAsync(
            string userId,
            string id,
            CancellationToken cancellationToken
        )
        {
            var mongoDatabase =
                mongoDbClient
                    .GetDatabase();

            var workflowCollection =
                mongoDatabase
                    .GetCollection<Workflow>(MongoConstants.WorkflowsCollection);

            var filter =
                Builders<Workflow>
                    .Filter
                    .Eq(
                        o => o.UserId,
                        userId
                    );

            filter &=
                Builders<Workflow>
                    .Filter
                    .Eq(
                        o => o.Id,
                        id
                    );

            var record =
                await
                    workflowCollection
                        .DeleteOneAsync(
                            filter,
                            cancellationToken
                        );

            return
                record
                    .DeletedCount > 0;
        }

        public async Task UpdateWorkflowFieldAsync<TField>(
            string workflowId,
            string userId,
            Expression<Func<Workflow, TField>> setter,
            TField value,
            CancellationToken cancellationToken
        )
        {
            try
            {
                var filter =
                    Builders<Workflow>
                        .Filter
                        .Eq(
                            u => u.UserId,
                            userId
                        );

                filter &=
                    Builders<Workflow>
                        .Filter
                        .Eq(
                            o => o.Id,
                            workflowId
                        );

                var update =
                    Builders<Workflow>
                        .Update
                        .Set(
                            setter,
                            value
                        );

                await
                    UpdateWorkflowAsync
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
                        "Error updating last login for user {userId}: {error}",
                        userId,
                        ex.Message
                    );
                throw;
            }
        }

        public async Task<bool> WorkflowExistsForUserAsync(
            string userId,
            string workflowName,
            CancellationToken cancellationToken
        )
        {
            var result =
                await
                    FetchWorkflowByNameAsync(
                        userId,
                        workflowName,
                        cancellationToken
                    ) != null;

            return result;
        }

        private async Task UpdateWorkflowAsync(
            FilterDefinition<Workflow> filter,
            UpdateDefinition<Workflow> update,
            CancellationToken cancellationToken
        )
        {
            var mongoDatabase =
                mongoDbClient
                    .GetDatabase();

            var workflowCollection =
                mongoDatabase
                    .GetCollection<Workflow>(MongoConstants.WorkflowsCollection);

            await
                workflowCollection
                    .UpdateOneAsync(
                        filter,
                        update,
                        new UpdateOptions(),
                        cancellationToken
                    );
        }

        private async Task<Workflow> FetchWorkflowByFilterAsync(
            FilterDefinition<Workflow> filter,
            CancellationToken cancellationToken
        )
        {
            try
            {
                var mongoDatabase =
                    mongoDbClient
                        .GetDatabase();

                var workflowCollection =
                    mongoDatabase
                        .GetCollection<Workflow>(MongoConstants.WorkflowsCollection);


                var record =
                    await
                        workflowCollection
                            .Find(filter)
                            .FirstOrDefaultAsync(cancellationToken: cancellationToken);

                return record;
            }
            catch (Exception ex)
            {
                logger
                    .LogError(
                        "There was an fetching workflow: {error}",
                        ex.Message
                    );

                throw;
            }
        }
    }
}