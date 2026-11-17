using System.Threading;
using System.Threading.Tasks;
using Rocket.Domain.Extensions;
using Rocket.Interfaces;

namespace Rocket.Infrastructure
{
    public class SafeFileRetitler(
        IBatchRepository batchRepository
    ) : IFileRetitler
    {
        public async Task<string> RetitleAsync(
            string rawTextData,
            string batchId,
            string userId,
            CancellationToken cancellationToken
        )
        {
            var sanitised =
                rawTextData
                    .ToSanitizedFileName();

            // Update the description of the batch
            await
                batchRepository
                    .UpdateBatchFieldAsync(
                        userId,
                        batchId,
                        o => o.Description,
                        sanitised,
                        cancellationToken
                    );

            return sanitised;
        }
    }
}