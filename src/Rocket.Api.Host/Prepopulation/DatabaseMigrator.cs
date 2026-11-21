using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Rocket.Interfaces;

namespace Rocket.Api.Host.Prepopulation
{
    public class DatabaseMigrator(
        ILogger<DatabaseMigrator> logger,
        IWorkflowRepository workflowRepository,
        IExecutionRepository executionRepository,
        IBatchRepository batchRepository
    ) : IDatabasePreRunAction
    {
        public async Task RunAsync(
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

            // --------------------------------------------------------
            // #134 - Drop thumbnail and content type from execution
            // --------------------------------------------------------
            await
                executionRepository
                    .SafelyDropColumnAsync(
                        "ThumbnailBase64",
                        cancellationToken
                    );

            await
                executionRepository
                    .SafelyDropColumnAsync(
                        "ContentType",
                        cancellationToken
                    );

            await
                executionRepository
                    .SafelyDropColumnAsync(
                        "Vendor",
                        cancellationToken
                    );

            await
                executionRepository
                    .SafelyDropColumnAsync(
                        "ScanId",
                        cancellationToken
                    );

            // --------------------------------------------------------
            // #177 - Checking two places for archived batches, remove archived flag
            // --------------------------------------------------------
            await
                batchRepository
                    .SafelyDropColumnAsync(
                        "Archived",
                        cancellationToken
                    );


            // --------------------------------------------------------
        }
    }
}