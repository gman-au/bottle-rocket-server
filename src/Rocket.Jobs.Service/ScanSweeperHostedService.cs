using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Rocket.Interfaces;

namespace Rocket.Jobs.Service
{
    public class ScanSweeperHostedService(
        ILogger<ScanSweeperHostedService> logger,
        ICaptureSweeper captureSweeper
    ) : BackgroundService
    {
        private const int PollingIntervalSeconds = 15;

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var (scansArchived, executionsArchived) =
                        await
                            captureSweeper
                                .PerformAsync(cancellationToken);

                    logger
                        .LogInformation(
                            "Archived {scansArchived} scans and {executionsArchived} executions",
                            scansArchived,
                            executionsArchived
                        );
                }
                catch (Exception ex)
                {
                    logger
                        .LogError(
                            ex,
                            "Error encountered while executing capture sweep"
                        );
                }
                finally
                {
                    await
                        Task
                            .Delay(
                                TimeSpan
                                    .FromSeconds(PollingIntervalSeconds),
                                cancellationToken
                            );
                }
            }
        }
    }
}