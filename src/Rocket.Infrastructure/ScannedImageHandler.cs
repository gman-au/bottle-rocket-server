using System.Threading;
using System.Threading.Tasks;
using Rocket.Interfaces;

namespace Rocket.Infrastructure
{
    public class ScannedImageHandler(IScannedImageRepository scannedImageRepository) : IScannedImageHandler
    {
        public async Task HandleAsync(byte[] toArray, CancellationToken cancellationToken)
        {
            await
                scannedImageRepository
                    .SaveCaptureAsync();
        }
    }
}