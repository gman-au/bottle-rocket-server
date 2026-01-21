using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Rocket.Api.Host.Hubs;
using Rocket.Interfaces;

namespace Rocket.Api.Host.Notifiers
{
    public class CaptureNotifier(
        IHubContext<CaptureHub> hubContext,
        ILogger<CaptureNotifier> logger
    )
        : ICaptureNotifier
    {
        public async Task NotifyNewCaptureAsync(
            string userId,
            CancellationToken cancellationToken
        )
        {
            logger
                .LogInformation(
                    "Notifying user {userId} of new capture",
                    userId
                );

            await
                hubContext
                    .Clients
                    .Group($"user_{userId}")
                    .SendAsync(
                        "NewCaptureReceived",
                        cancellationToken: cancellationToken
                    );
        }

        public async Task NotifyNewExecutionUpdateAsync(
            string userId,
            CancellationToken cancellationToken
        )
        {
            logger
                .LogInformation(
                    "Notifying user {userId} of new execution update",
                    userId
                );

            await
                hubContext
                    .Clients
                    .Group($"user_{userId}")
                    .SendAsync(
                        "NewExecutionUpdateReceived",
                        cancellationToken: cancellationToken
                    );
        }
    }
}