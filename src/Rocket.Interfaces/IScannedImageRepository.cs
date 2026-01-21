using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Rocket.Domain;

namespace Rocket.Interfaces
{
    public interface IScannedImageRepository
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
    }
}