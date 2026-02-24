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
        ICaptureSweeper captureSweeper,
        IGlobalSettingsRepository globalSettingsRepository,
        IGlobalSettingsChangedSignal globalSettingsChangedSignal
    ) : BackgroundService
    {
        private const int PollingIntervalSeconds = 15;

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    // get settings
                    var globalSettings =
                        await
                            globalSettingsRepository
                                .GetGlobalSettingsAsync(cancellationToken);

                    var sweepEnabled =
                        globalSettings?
                            .EnableSweeping ?? false;

                    var daysSinceLastSuccessfulExecution =
                        globalSettings?
                            .SweepSuccessfulScansAfterDays ?? 7;

                    await
                        captureSweeper
                            .PerformAsync(
                                sweepEnabled,
                                daysSinceLastSuccessfulExecution,
                                cancellationToken
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
                            .WhenAny(
                                Task
                                    .Delay(
                                        TimeSpan
                                            .FromSeconds(PollingIntervalSeconds),
                                        cancellationToken
                                    ),
                                globalSettingsChangedSignal
                                    .WaitAsync(cancellationToken)
                            );
                }
            }
        }
    }
}