using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Rocket.Domain.Enum;
using Rocket.Domain.Exceptions;
using Rocket.Interfaces;

namespace Rocket.Infrastructure
{
    public class ExecutionScheduler(
        ILogger<ExecutionScheduler> logger,
        IWorkflowExecutionManager workflowExecutionManager,
        IBatchRepository batchRepository,
        IExecutionRepository executionRepository,
        IWorkflowRepository workflowRepository,
        IWorkflowCloner workflowCloner,
        IExecutionWorkflowValidator executionWorkflowValidator
    ) : IExecutionScheduler
    {
        public async Task<string> ScheduleExecutionAsync(
            string workflowId,
            string batchId,
            string userId,
            bool runImmediately,
            bool throwOnFailure,
            CancellationToken cancellationToken
        )
        {
            try
            {
                if (string.IsNullOrEmpty(batchId))
                    throw new RocketException(
                        "No batch ID was provided.",
                        ApiStatusCodeEnum.ValidationError
                    );

                var workflow =
                    await
                        workflowRepository
                            .GetWorkflowByIdAsync(
                                userId,
                                workflowId,
                                cancellationToken
                            );

                if (workflow == null)
                    throw new RocketException(
                        "Workflow does not exist for this user.",
                        ApiStatusCodeEnum.UnknownOrInaccessibleRecord
                    );

                if (!(workflow.Steps ?? []).Any())
                    throw new RocketException(
                        "This workflow has no steps defined. Please check the workflow configuration.",
                        ApiStatusCodeEnum.ValidationError
                    );

                var missingConnectors =
                    executionWorkflowValidator
                        .GetMissingConnectors(workflow)
                        .ToList();

                if (missingConnectors.Count != 0)
                    throw new RocketException(
                        "This workflow contains one or more steps with missing connectors: " +
                        $"{string.Join(", ", missingConnectors.Select(o => $"'{o}'"))}.",
                        ApiStatusCodeEnum.ValidationError
                    );

                var batch =
                    await
                        batchRepository
                            .GetBatchByIdAsync(
                                userId,
                                batchId,
                                cancellationToken
                            );

                if (batch == null)
                    throw new RocketException(
                        "Batch does not exist for this user.",
                        ApiStatusCodeEnum.UnknownOrInaccessibleRecord
                    );

                if (batch.Archived)
                    throw new RocketException(
                        "Batch is archived and cannot be processed.",
                        ApiStatusCodeEnum.RecordIsArchived
                    );

                if (!(batch.Scans ?? []).Any())
                    throw new RocketException(
                        "Batch contains no scanned images to process.",
                        ApiStatusCodeEnum.NoAttachmentsFound
                    );

                var coverThumbnailBase64 =
                    batch
                        .Scans
                        .First()
                        .ThumbnailBase64;
                
                var coverContentType =
                    batch
                        .Scans
                        .First()
                        .ContentType;
                
                var coverVendor =
                    batch
                        .Scans
                        .First()
                        .Vendor;

                var newExecution =
                    workflowCloner
                        .Clone(
                            workflow,
                            batch.Id,
                            coverThumbnailBase64,
                            coverContentType,
                            coverVendor
                        );

                var result =
                    await
                        executionRepository
                            .InsertExecutionAsync(
                                newExecution,
                                cancellationToken
                            );

                if (result == null)
                    throw new RocketException(
                        "Failed to create execution",
                        ApiStatusCodeEnum.ServerError
                    );

                var executionId =
                    result
                        .Id;

                if (!runImmediately) return executionId;

                logger
                    .LogInformation(
                        "Scheduling and running execution with ID: {executionId}",
                        executionId
                    );

                await
                    workflowExecutionManager
                        .StartExecutionAsync(
                            executionId,
                            userId,
                            cancellationToken
                        );

                return executionId;
            }
            catch (RocketException ex)
            {
                if (throwOnFailure) throw;

                logger
                    .LogWarning(
                        "Could not schedule workflow ID {workflowId} for scan ID {scanId} for user ID {userId} - {error}",
                        workflowId,
                        batchId,
                        userId,
                        ex.Message
                    );

                return null;
            }
        }
    }
}