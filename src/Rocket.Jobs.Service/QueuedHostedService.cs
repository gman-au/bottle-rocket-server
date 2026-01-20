using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Rocket.Interfaces;

namespace Rocket.Jobs.Service
{
    public class QueuedHostedService(IBackgroundTaskQueue taskQueue) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var workItem =
                    await
                        taskQueue
                            .DequeueAsync(cancellationToken);

                await
                    workItem(cancellationToken);
            }
        }
    }
}