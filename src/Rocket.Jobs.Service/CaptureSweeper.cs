using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Rocket.Domain.Enum;
using Rocket.Domain.Exceptions;
using Rocket.Interfaces;

namespace Rocket.Jobs.Service
{
    public class CaptureSweeper(
        ILogger<CaptureSweeper> logger,
        IScannedImageHandler scannedImageHandler,
        IExecutionRepository executionRepository
    ) : ICaptureSweeper
    {
        private const int DefaultDaysSinceLastSuccessfulExecution = 7;

        public async Task<(int, int)> PerformAsync(CancellationToken cancellationToken)
        {
            logger
                .LogInformation("Retrieving successful executions");

            var pendingExecutions =
                await
                    executionRepository
                        .GetExecutionSuccessesForOlderScansAsync(
                            DefaultDaysSinceLastSuccessfulExecution,
                            cancellationToken
                        );

            var totalScansArchived = 0;
            var totalExecutionsArchived = 0;

            foreach (var pendingExecution in pendingExecutions)
            {
                try
                {
                    // archive the execution
                    await
                        executionRepository
                            .UpdateExecutionFieldAsync(
                                pendingExecution.Id,
                                pendingExecution.UserId,
                                o => o.Archived,
                                true,
                                cancellationToken
                            );

                    totalExecutionsArchived++;

                    // archive the scan
                    await
                        scannedImageHandler
                            .ArchiveAsync(
                                pendingExecution.UserId,
                                pendingExecution.ScanId,
                                cancellationToken
                            );

                    logger
                        .LogInformation("Successfully archived scan ID: {scanId}", pendingExecution.ScanId);

                    totalScansArchived++;
                }
                catch (RocketException ex)
                {
                    if (ex.ApiStatusCode == (int)ApiStatusCodeEnum.RecordIsArchived)
                        continue;

                    throw;
                }
                catch (Exception ex)
                {
                    logger
                        .LogError(ex, "Error archiving execution ID: {executionId}", pendingExecution);
                }
            }

            return (totalScansArchived, totalExecutionsArchived);
        }
    }
}