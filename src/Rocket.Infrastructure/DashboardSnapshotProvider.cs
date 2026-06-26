using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Rocket.Domain.Dashboard;
using Rocket.Domain.Enum;
using Rocket.Interfaces;

namespace Rocket.Infrastructure
{
    public class DashboardSnapshotProvider(
        ILogger<DashboardSnapshotProvider> logger,
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
            {
                logger
                    .LogInformation("Returning cached dashboard snapshot for user {userId}", userId);
                
                return cachedSnapshot;
            }

            var result = new DashboardSnapshot();

            // Scans by vendor
            var scansByVendor =
                (await
                    scannedImageRepository
                        .AggregateScansByVendorAsync(
                            userId,
                            cancellationToken
                        ))
                .OrderBy(o => o.Vendor)
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
                .OrderBy(o => o.Workflow)
                .ToList();

            // Executions by status
            var executionsByStatus =
                (await
                    executionRepository
                        .AggregateExecutionsByStatusAsync(
                            userId,
                            cancellationToken
                        ))
                .OrderBy(o => o.Status)
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
            
            // Lifecycle totals
            var executionLifecycleTotals =
                await
                    executionRepository
                        .AggregateLifecycleTotalsAsync(
                            userId,
                            cancellationToken
                        );

            result.Lifecycles =
                new LifecycleSummary
                {
                    LifecyclesByGroup = 
                        executionLifecycleTotals
                            .Select(o => new LifecycleTotal
                                {
                                    Workflow = o.Workflow,
                                    Status = o.Status,
                                    Count = o.Count
                                }
                            )
                };

            var hasRunningExecutions =
                executionsByStatus
                    .Any(o => o.Status == (int)ExecutionStatusEnum.Running);
            
            // If there is a 'running' status, do not cache
            if (!hasRunningExecutions)
            {
                logger
                    .LogInformation("Caching dashboard snapshot for user {userId}", userId);
                
                cache
                    .Set(
                        userId,
                        result,
                        TimeSpan
                            .FromMinutes(CacheExpiryMinutes)
                    );
            }
            else
            {
                logger
                    .LogWarning("Dashboard snapshot for user {userId} has running workflows - not caching", userId);
            }

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