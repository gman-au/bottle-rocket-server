using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Rocket.Domain.Core.Enum;
using Rocket.Domain.Exceptions;
using Rocket.Interfaces;

namespace Rocket.Infrastructure
{
    public class ExecutionScheduler(
        ILogger<ExecutionScheduler> logger,
        IWorkflowExecutionManager workflowExecutionManager,
        IScannedImageRepository scannedImageRepository,
        IExecutionRepository executionRepository,
        IWorkflowRepository workflowRepository,
        IWorkflowCloner workflowCloner
    ) : IExecutionScheduler
    {
        public async Task<string> ScheduleExecutionAsync(
            string workflowId,
            string scanId,
            string userId,
            bool runImmediately,
            CancellationToken cancellationToken
        )
        {
            if (string.IsNullOrEmpty(scanId))
                throw new RocketException(
                    "No scan ID was provided.",
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

            var scan =
                await
                    scannedImageRepository
                        .GetScanByIdAsync(
                            userId,
                            scanId,
                            cancellationToken
                        );

            if (scan == null)
                throw new RocketException(
                    "Scan does not exist for this user.",
                    ApiStatusCodeEnum.UnknownOrInaccessibleRecord
                );

            var newExecution =
                workflowCloner
                    .Clone(
                        workflow,
                        scan.Id,
                        scan.ThumbnailBase64,
                        scan.ContentType
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
                .LogInformation("Scheduling and running execution with ID: {executionId}", executionId);
                
            await
                workflowExecutionManager
                    .StartExecutionAsync(
                        executionId,
                        userId,
                        cancellationToken
                    );

            return executionId;
        }
    }
}