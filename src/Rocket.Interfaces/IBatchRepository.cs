using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Rocket.Domain.Batches;
using Rocket.Domain.Dashboard;
using Rocket.Domain.ScannedImages;

namespace Rocket.Interfaces
{
    public interface IBatchRepository : IMigratableRepository
    {
        Task<Batch> GetBatchByIdAsync(
            string userId,
            string id,
            CancellationToken cancellationToken
        );

        Task<(IEnumerable<Batch> records, long totalRecordCount)> FetchBatchesAsync(
            string userId,
            int startIndex,
            int recordCount,
            CancellationToken cancellationToken
        );

        Task<Batch> InsertBatchAsync(
            Batch batch,
            CancellationToken cancellationToken
        );

        Task ArchiveBatchAsync(
            string userId,
            string batchId,
            CancellationToken cancellationToken
        );

        Task DeleteBatchAsync(
            string userId,
            string id,
            CancellationToken cancellationToken
        );

        Task UpdateBatchFieldAsync<TField>(
            string userId,
            string id,
            Expression<Func<Batch, TField>> setter,
            TField value,
            CancellationToken cancellationToken
        );

        Task<ScannedImage> InsertBatchScanAsync(
            ScannedImage scannedImage,
            string userId,
            string batchId,
            CancellationToken cancellationToken
        );

        Task<IEnumerable<ScanByVendorTotal>> AggregateScansByVendorAsync(
            string userId,
            CancellationToken cancellationToken
        );

        Task<ScannedImage> GetScanByIdAsync(
            string userId,
            string batchId,
            string scanId,
            CancellationToken cancellationToken
        );

        Task DeleteScanByIdAsync(
            string userId,
            string batchId,
            string scanId,
            CancellationToken cancellationToken
        );
    }
}