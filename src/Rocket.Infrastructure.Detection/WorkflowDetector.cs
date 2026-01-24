using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Rocket.Domain.Core.Enum;
using Rocket.Interfaces;

namespace Rocket.Infrastructure.Detection
{
    public class WorkflowDetector(
        ILogger<WorkflowDetector> logger,
        ISymbolDetector symbolDetector,
        IExecutionScheduler executionScheduler,
        IWorkflowRepository workflowRepository
    ) : IWorkflowDetector
    {
        public async Task DetectAndScheduleWorkflowAsync(
            string scanId,
            string userId,
            string modelQrCode,
            string modelQrBoundingBox,
            byte[] imageBytes,
            CancellationToken cancellationToken
        )
        {
            var detectedSymbols =
                await
                    symbolDetector
                        .DetectSymbolMarksAsync(
                            modelQrCode,
                            modelQrBoundingBox,
                            imageBytes,
                            cancellationToken
                        );

            if ((detectedSymbols ?? []).Length != 0)
            {
                foreach (var detectedSymbol in detectedSymbols!)
                {
                    var matchedWorkflow =
                        await
                            workflowRepository
                                .GetWorkflowByMatchingPageSymbolAsync(
                                    userId,
                                    detectedSymbol,
                                    cancellationToken
                                );

                    if (matchedWorkflow == null) continue;
                    
                    logger
                        .LogInformation(
                            "Workflow matched for scan ID {scanId}, user {userId}, symbol {symbol}; creating and running execution",
                            scanId,
                            userId,
                            Enum.GetName(typeof(PageSymbolEnum), detectedSymbol)
                        );

                    await
                        executionScheduler
                            .ScheduleExecutionAsync(
                                matchedWorkflow.Id,
                                scanId,
                                userId,
                                true,
                                cancellationToken
                            );
                }
            }
        }
    }
}