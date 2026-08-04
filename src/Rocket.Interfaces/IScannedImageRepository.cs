using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Rocket.Domain;
using Rocket.Domain.Dashboard;

namespace Rocket.Interfaces
{
    public interface IScannedImageRepository : IMigratableRepository
    {
        Task<ScannedImage> GetScanByIdAsync(
            string userId,
            string id,
            CancellationToken cancellationToken
        );

        Task<(IEnumerable<ScannedImage> records, long totalRecordCount)> FetchScansAsync(
            string userId,
            int startIndex,
            int recordCount,
            CancellationToken cancellationToken
        );

        Task<ScannedImage> InsertScanAsync(
            ScannedImage scannedImage,
            CancellationToken cancellationToken
        );

        Task ArchiveScanAsync(
            string userId,
            string id,
            CancellationToken cancellationToken
        );

        Task DeleteScanAsync(
            string userId,
            string id,
            CancellationToken cancellationToken
        );

        Task UpdateScannedImageFieldAsync<TField>(
            string userId,
            string id,
            Expression<Func<ScannedImage, TField>> setter,
            TField value,
            CancellationToken cancellationToken
        );

        Task<IEnumerable<ScanByVendorTotal>> AggregateScansByVendorAsync(
            string userId,
            CancellationToken cancellationToken
        );
    }
}