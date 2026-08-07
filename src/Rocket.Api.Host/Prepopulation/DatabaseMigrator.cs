using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Rocket.Interfaces;

namespace Rocket.Api.Host.Prepopulation
{
    public class DatabaseMigrator(
        ILogger<DatabaseMigrator> logger,
        IScannedImageRepository scannedImageRepository,
        IWorkflowRepository workflowRepository,
        IExecutionRepository executionRepository
    ) : IDatabaseMigrator
    {
        public async Task ApplyMigrationsAsync(
            CancellationToken cancellationToken
        )
        {
            logger
                .LogWarning("Applying column drops, where needed...");

            // --------------------------------------------------------
            // #124 - Drop matching page symbol column if it exists
            // --------------------------------------------------------
            await
                workflowRepository
                    .SafelyDropColumnAsync(
                        "MatchingPageSymbol",
                        cancellationToken
                    );
            
            await
                executionRepository
                    .SafelyDropColumnAsync(
                        "MatchingPageSymbol",
                        cancellationToken
                    );
            
            await
                scannedImageRepository
                    .SafelyDropColumnAsync(
                        "QrCode",
                        cancellationToken
                    );
            
            await
                scannedImageRepository
                    .SafelyDropColumnAsync(
                        "QrBoundingBox",
                        cancellationToken
                    );
            // --------------------------------------------------------
        }
    }
}