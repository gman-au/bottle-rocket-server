using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Rocket.Domain.Enum;
using Rocket.Interfaces;

namespace Rocket.Infrastructure.Detection
{
    [Obsolete("This detector is deprecated and will be removed in a future release.")]
    public class OpticalWorkflowDetector(
        ILogger<OpticalWorkflowDetector> logger,
        ISymbolDetector symbolDetector,
        IExecutionScheduler executionScheduler,
        IWorkflowRepository workflowRepository
    ) : IWorkflowDetector
    {
        public async Task DetectAndScheduleWorkflowAsync(
            string scanId,
            string userId,
            string vendor,
            string modelQrCode,
            string modelQrBoundingBox,
            byte[] imageBytes,
            IEnumerable<string> workflowIds,
            CancellationToken cancellationToken
        )
        {
            var detectedSymbols =
                await
                    symbolDetector
                        .DetectSymbolMarksAsync(
                            modelQrCode,
                            modelQrBoundingBox,
                            vendor,
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
                                    workflowIdToExclude: null,
                                    detectedSymbol,
                                    cancellationToken
                                );

                    if (matchedWorkflow == null) continue;

                    logger
                        .LogInformation(
                            "Workflow matched for scan ID {scanId}, user {userId}, symbol {symbol}; creating and running execution",
                            scanId,
                            userId,
                            Enum.GetName(
                                typeof(PageSymbolEnum),
                                detectedSymbol
                            )
                        );

                    await
                        executionScheduler
                            .ScheduleExecutionAsync(
                                matchedWorkflow.Id,
                                scanId,
                                userId,
                                runImmediately: true,
                                throwOnFailure: false,
                                cancellationToken
                            );
                }
            }
        }
    }
}