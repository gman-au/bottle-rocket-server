using System.Threading;
using System.Threading.Tasks;
using Rocket.Domain.Extensions;
using Rocket.Interfaces;

namespace Rocket.Infrastructure
{
    public class SafeFileRetitler(
        IScannedImageRepository scannedImageRepository
    ) : IFileRetitler
    {
        public async Task<string> RetitleAsync(
            string rawTextData,
            string scanId,
            string userId,
            CancellationToken cancellationToken
        )
        {
            var sanitised =
                rawTextData
                    .ToSanitizedFileName();

            // Update the description of the scan
            await
                scannedImageRepository
                    .UpdateScannedImageFieldAsync(
                        userId,
                        scanId,
                        o => o.Description,
                        sanitised,
                        cancellationToken
                    );

            return sanitised;
        }
    }
}