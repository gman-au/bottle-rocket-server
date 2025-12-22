using System.Threading;
using System.Threading.Tasks;
using Rocket.Domain;

namespace Rocket.Interfaces
{
    public interface IScannedImageRepository
    {
        Task<ScannedImage> SaveCaptureAsync(
            ScannedImage scannedImage,
            CancellationToken cancellationToken
        );
    }
}