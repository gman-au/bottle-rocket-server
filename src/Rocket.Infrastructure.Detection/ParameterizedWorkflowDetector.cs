using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Rocket.Interfaces;

namespace Rocket.Infrastructure.Detection
{
    public class ParameterizedWorkflowDetector(
        ILogger<ParameterizedWorkflowDetector> logger,
        IExecutionScheduler executionScheduler,
        IWorkflowRepository workflowRepository
    ) : IWorkflowDetector
    {
        public async Task DetectAndScheduleWorkflowAsync(
            string batchId,
            string userId,
            IEnumerable<string> workflowIds,
            CancellationToken cancellationToken
        )
        {
            if ((workflowIds ?? []).Any())
            {
                foreach (var workflowId in workflowIds!)
                {
                    var matchedWorkflow =
                        await
                            workflowRepository
                                .GetWorkflowByIdAsync(
                                    userId,
                                    workflowId,
                                    cancellationToken
                                );

                    if (matchedWorkflow != null)
                    {
                        logger
                            .LogInformation(
                                "Workflow matched for scan ID {scanId}, user {userId}, workflow ID {workflowId}; creating and running execution",
                                batchId,
                                userId,
                                workflowId
                            );

                        await
                            executionScheduler
                                .ScheduleExecutionAsync(
                                    matchedWorkflow.Id,
                                    batchId,
                                    userId,
                                    runImmediately: true,
                                    throwOnFailure: false,
                                    cancellationToken
                                );
                    }
                    else
                    {
                        logger
                            .LogWarning(
                                "Workflow NOT matched for scan ID {scanId}, user {userId}, workflow ID {workflowId}",
                                batchId,
                                userId,
                                workflowId
                            );
                    }
                }
            }
        }
    }
}