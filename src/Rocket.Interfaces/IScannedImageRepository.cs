using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Rocket.Domain.Dashboard;
using Rocket.Domain.ScannedImages;

namespace Rocket.Interfaces
{
    public interface IScannedImageRepository : IMigratableRepository
    {
        Task<ScannedImage> GetScanByIdAsync(
            string userId,
            string id,
            CancellationToken cancellationToken
        );

        Task<ScannedImage> InsertScanAsync(
            ScannedImage scannedImage,
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