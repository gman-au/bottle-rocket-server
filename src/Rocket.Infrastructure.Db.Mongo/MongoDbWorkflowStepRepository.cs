using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using Rocket.Domain.Enum;
using Rocket.Domain.Exceptions;
using Rocket.Domain.Workflows;
using Rocket.Infrastructure.Db.Mongo.Extensions;
using Rocket.Interfaces;

namespace Rocket.Infrastructure.Db.Mongo
{
    public class MongoDbWorkflowStepRepository(
        ILogger<MongoDbWorkflowStepRepository> logger,
        IMongoDbClient mongoDbClient
    ) : MongoDbRepositoryBase<Workflow>(mongoDbClient, logger), IWorkflowStepRepository
    {
        protected override string CollectionName => MongoConstants.WorkflowsCollection;

        public async Task<BaseWorkflowStep> InsertWorkflowStepAsync(
            BaseWorkflowStep workflowStep,
            string userId,
            string workflowId,
            string parentStepId,
            CancellationToken cancellationToken
        )
        {
            var id =
                ObjectId
                    .GenerateNewId()
                    .ToString();

            workflowStep.Id = id;

            var filter =
                BuildWorkflowFilterByIdAndUserId(workflowId, userId);

            var options = new UpdateOptions();

            var workflow =
                await
                    GetWorkflowByIdAsync(
                        workflowId,
                        userId,
                        cancellationToken
                    );

            if (workflow == null)
                throw new RocketException(
                    $"Could not find workflow with id [{workflowId}]",
                    ApiStatusCodeEnum.UnknownOrInaccessibleRecord
                );

            if (string.IsNullOrEmpty(parentStepId))
            {
                var update =
                    Builders<Workflow>
                        .Update
                        .Push(w => w.Steps, workflowStep);

                await
                    GetMongoCollection()
                        .UpdateOneAsync(
                            filter,
                            update,
                            options,
                            cancellationToken
                        );
            }
            else
            {
                if (!workflow.Steps.AddChildToParent(workflowStep, ref parentStepId))
                    throw new RocketException(
                        $"Could not find parent record with id [{parentStepId}]",
                        ApiStatusCodeEnum.UnknownOrInaccessibleRecord
                    );

                await
                    GetMongoCollection()
                        .ReplaceOneAsync(
                            filter,
                            workflow,
                            cancellationToken: cancellationToken
                        );
            }

            return workflowStep;
        }

        public async Task<BaseWorkflowStep> GetWorkflowStepByIdAsync(
            string workflowStepId,
            string workflowId,
            string userId,
            CancellationToken cancellationToken = default
        )
        {
            var workflow =
                await
                    GetWorkflowByIdAsync(
                        workflowId,
                        userId,
                        cancellationToken
                    );

            return
                workflow
                    .Steps
                    .FindStepById(
                        workflowStepId
                    );
        }

        public async Task<bool> DeleteWorkflowStepAsync(
            string userId,
            string workflowId,
            string workflowStepId,
            CancellationToken cancellationToken
        )
        {
            var workflow =
                await
                    GetWorkflowByIdAsync(
                        workflowId,
                        userId,
                        cancellationToken
                    );

            // Check root level first
            var rootSteps =
                workflow
                    .Steps?
                    .ToList();

            if (rootSteps == null) return false;

            if (!workflow.DeleteStepById(workflowStepId))
                throw new RocketException(
                    $"Could not find workflow step with id [{workflowStepId}]",
                    ApiStatusCodeEnum.UnknownOrInaccessibleRecord
                );

            var filter =
                BuildWorkflowFilterByIdAndUserId(workflowId, userId);

            await
                GetMongoCollection()
                    .ReplaceOneAsync(
                        filter,
                        workflow,
                        cancellationToken: cancellationToken
                    );

            return true;
        }

        public async Task<Workflow> GetWorkflowByIdAsync(
            string workflowId,
            string userId,
            CancellationToken cancellationToken
        )
        {
            var filter =
                BuildWorkflowFilterByIdAndUserId(workflowId, userId);

            var workflow =
                await
                    GetMongoCollection()
                        .Find(filter)
                        .FirstOrDefaultAsync(cancellationToken);

            if (workflow == null)
                throw new RocketException(
                    $"Could not find workflow record with id [{workflowId}]",
                    ApiStatusCodeEnum.UnknownOrInaccessibleRecord
                );

            return workflow;
        }

        private static FilterDefinition<Workflow> BuildWorkflowFilterByIdAndUserId(
            string workflowId,
            string userId) =>
            Builders<Workflow>
                .Filter
                .Eq(
                    w => w.Id,
                    workflowId
                ) &
            Builders<Workflow>
                .Filter
                .Eq(
                    w => w.UserId,
                    userId
                );
    }
}