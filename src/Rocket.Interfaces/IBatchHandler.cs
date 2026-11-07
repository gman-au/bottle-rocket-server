using System.Threading;
using System.Threading.Tasks;
using Rocket.Domain.Batches;
using Rocket.Domain.ScannedImages;

namespace Rocket.Interfaces
{
    public interface IBatchHandler
    {
        Task<ScannedImage> WriteAsync(
            byte[] imageData,
            string contentType,
            string fileExtension,
            string userId,
            string vendor,
            string description,
            string batchId,
            CancellationToken cancellationToken
        );

        Task<Batch> OpenAsync(
            string userId,
            CancellationToken cancellationToken
        );

        Task CloseAsync(
            string userId,
            string batchId,
            CancellationToken cancellationToken
        );

        Task<(ScannedImage record, byte[] imageData)> ReadAsync(
            string userId,
            string id,
            CancellationToken cancellationToken
        );

        Task<bool> ArchiveAsync(
            string userId,
            string id,
            CancellationToken cancellationToken
        );

        Task<bool> DeleteAsync(
            string userId,
            string id,
            CancellationToken cancellationToken
        );
    }
}