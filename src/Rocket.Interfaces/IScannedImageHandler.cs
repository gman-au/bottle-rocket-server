using System.Threading;
using System.Threading.Tasks;
using Rocket.Domain;

namespace Rocket.Interfaces
{
    public interface IScannedImageHandler
    {
        Task<ScannedImage> WriteAsync(
            byte[] imageData,
            string contentType,
            string fileExtension,
            string userId,
            string qrCode,
            string qrBoundingBox,
            CancellationToken cancellationToken
        );

        Task<(ScannedImage record, byte[] imageData)> ReadAsync(
            string userId,
            string id,
            CancellationToken cancellationToken
        );
    }
}