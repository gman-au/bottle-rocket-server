using System.Threading;
using System.Threading.Tasks;
using Rocket.Domain.ScannedImages;

namespace Rocket.Interfaces
{
    public interface IScannedImageHandler
    {
        Task<(ScannedImage record, byte[] imageData)> ReadFromBatchAsync(
            string userId,
            string batchId,
            string scanId,
            CancellationToken cancellationToken
        );

        Task<bool> DeleteFromBatchAsync(
            string userId,
            string batchId,
            string scanId,
            CancellationToken cancellationToken
        );
    }
}