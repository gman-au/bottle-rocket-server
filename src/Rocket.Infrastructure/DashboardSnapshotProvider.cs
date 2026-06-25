using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Rocket.Domain.Dashboard;
using Rocket.Interfaces;

namespace Rocket.Infrastructure
{
    public class DashboardSnapshotProvider(
        IScannedImageRepository scannedImageRepository,
        IExecutionRepository executionRepository,
        IBlobStore blobStore,
        IMemoryCache cache
    ) : IDashboardSnapshotProvider
    {
        private const int CacheExpiryMinutes = 10;

        public async Task<DashboardSnapshot> GetSnapshotForUserAsync(
            string userId,
            CancellationToken cancellationToken
        )
        {
            if (cache.TryGetValue(
                    userId,
                    out DashboardSnapshot cachedSnapshot
                ))
                return cachedSnapshot;

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

            // Executions by workflow
            var executionsByWorkflow =
                (await
                    executionRepository
                        .AggregateExecutionsByWorkflowAsync(
                            userId,
                            cancellationToken
                        ))
                .ToList();

            // Executions by status
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

            // Storage
            var storage =
                await
                    blobStore
                        .GetDriveInfoAsync(cancellationToken);

            result.Storage =
                new StorageSummary
                {
                    UsedStorageBytes = storage.Item1,
                    AvailableStorageBytes = storage.Item2
                };

            cache
                .Set(
                    userId,
                    result,
                    TimeSpan
                        .FromMinutes(CacheExpiryMinutes)
                );

            return result;
        }

        public void MarkAsDirty(
            string userId
        )
        {
            if (cache.TryGetValue(
                    userId,
                    out _
                ))
                cache
                    .Remove(userId);
        }
    }
}