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
        IDashboardSnapshotProvider dashboardSnapshotProvider,
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

            dashboardSnapshotProvider
                .MarkAsDirty(userId);

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

            dashboardSnapshotProvider
                .MarkAsDirty(userId);

            await
                hubContext
                    .Clients
                    .Group($"user_{userId}")
                    .SendAsync(
                        "NewExecutionUpdateReceived",
                        cancellationToken: cancellationToken
                    );
        }

        public async Task NotifyConnectorUpdateAsync(
            string userId,
            bool success,
            CancellationToken cancellationToken
        )
        {
            logger
                .LogInformation(
                    "Notifying user {userId} of new connector update",
                    userId
                );

            await
                hubContext
                    .Clients
                    .Group($"user_{userId}")
                    .SendAsync(
                        "NewConnectorUpdateReceived",
                        success,
                        cancellationToken: cancellationToken
                    );
        }

        public async Task NotifyScanDeleteAsync(
            string userId,
            CancellationToken cancellationToken
        )
        {
            logger
                .LogInformation(
                    "Notifying user {userId} of scan delete",
                    userId
                );

            await
                hubContext
                    .Clients
                    .Group($"user_{userId}")
                    .SendAsync(
                        "ScanDeletedReceived",
                        cancellationToken: cancellationToken
                    );
        }

        public async Task NotifyExecutionDeleteAsync(
            string userId,
            CancellationToken cancellationToken
        )
        {
            logger
                .LogInformation(
                    "Notifying user {userId} of execution delete",
                    userId
                );

            await
                hubContext
                    .Clients
                    .Group($"user_{userId}")
                    .SendAsync(
                        "ExecutionDeletedReceived",
                        cancellationToken: cancellationToken
                    );
        }
    }
}