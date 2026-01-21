using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Rocket.Interfaces;

namespace Rocket.Jobs.Service
{
    public class QueuedHostedService(
        IBackgroundTaskQueue taskQueue,
        ILogger<QueuedHostedService> logger
    ) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var workItem =
                        await
                            taskQueue
                                .DequeueAsync(cancellationToken);

                    await
                        workItem(cancellationToken);
                }
                catch (Exception ex)
                {
                    logger
                        .LogError(
                            ex,
                            "Error encountered while executing queued work item"
                        );

                    await
                        Task
                            .Delay(
                                TimeSpan
                                    .FromSeconds(2),
                                cancellationToken
                            );
                }
            }
        }
    }
}