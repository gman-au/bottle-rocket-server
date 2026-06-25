using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Rocket.Domain.Dashboard;
using Rocket.Interfaces;

namespace Rocket.Infrastructure
{
    public class DashboardSnapshotProvider(
        IScannedImageRepository scannedImageRepository,
        IExecutionRepository executionRepository
    ) : IDashboardSnapshotProvider
    {
        public async Task<DashboardSnapshot> GetSnapshotForUserAsync(
            string userId,
            CancellationToken cancellationToken
        )
        {
            // TODO: get cached value prior - set this up
            var result = new DashboardSnapshot();

            // Scans by vendor
            var scansByVendor =
                (await
                    scannedImageRepository
                        .AggregateScansByVendorAsync(
                            userId,
                            cancellationToken
                        ))
                .ToList();

            result.Scans =
                new ScansSummary
                {
                    TotalScansReceived =
                        scansByVendor
                            .Sum(o => o.Scans),
                    ScansReceivedByVendor = scansByVendor
                };

            var executionsByWorkflow =
                (await
                    executionRepository
                        .AggregateExecutionsByWorkflowAsync(
                            userId,
                            cancellationToken
                        ))
                .ToList();

            var executionsByStatus =
                (await
                    executionRepository
                        .AggregateExecutionsByStatusAsync(
                            userId,
                            cancellationToken
                        ))
                .ToList();

            result.Executions =
                new ExecutionsSummary
                {
                    TotalExecutions =
                        executionsByWorkflow
                            .Sum(o => o.Executions),
                    ExecutionsByWorkflow = executionsByWorkflow,
                    ExecutionsByStatus = executionsByStatus
                };

            return result;
        }
    }
}