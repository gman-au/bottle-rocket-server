using System;
using System.Threading;
using System.Threading.Tasks;

namespace Rocket.Interfaces
{
    public interface ITranscriber
    {
        Task ApplyTranscriptionAsync(
            string userId,
            string batchId,
            string scanId,
            string executionId,
            string executionName,
            DateTime executionDate,
            string text,
            CancellationToken cancellationToken
        );

        Task ClearTranscriptionAsync(
            string userId,
            string batchId,
            string scanId,
            CancellationToken cancellationToken
        );
    }
}