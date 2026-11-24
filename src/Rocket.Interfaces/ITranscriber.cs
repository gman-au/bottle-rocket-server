using System;
using System.Threading;
using System.Threading.Tasks;
using Rocket.Api.Contracts.Transcriptions;

namespace Rocket.Interfaces
{
    public interface ITranscriber
    {
        Task ApplyTranscriptionAsync<T>(
            string userId,
            string batchId,
            string scanId,
            T transcriptionSummary,
            CancellationToken cancellationToken
        ) where T : TranscriptionSummary;

        Task ClearTranscriptionAsync(
            string userId,
            string batchId,
            string scanId,
            CancellationToken cancellationToken
        );
    }
}