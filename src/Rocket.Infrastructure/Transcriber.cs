using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Rocket.Api.Contracts.Transcriptions;
using Rocket.Interfaces;

namespace Rocket.Infrastructure
{
    public class Transcriber(
        ILogger<Transcriber> logger,
        ITranscriptionModelMapperRegistry transcriptionModelMapperRegistry,
        IBatchRepository batchRepository
    ) : ITranscriber
    {
        public async Task ApplyTranscriptionAsync<T>(
            string userId,
            string batchId,
            string scanId,
            T transcriptionSummary,
            CancellationToken cancellationToken
        ) where T : TranscriptionSummary
        {
            logger
                .LogInformation(
                    "Updating transcription record for batch ID: {batchId}, scan ID: {scanId}",
                    batchId,
                    scanId
                );

            var mapper =
                transcriptionModelMapperRegistry
                    .GetMapperForView(transcriptionSummary.GetType());

            var newTranscription =
                mapper
                    .For(transcriptionSummary);

            await
                batchRepository
                    .UpdateBatchScanTranscriptionAsync(
                        userId,
                        batchId,
                        scanId,
                        newTranscription,
                        cancellationToken
                    );
            
            logger
                .LogInformation(
                    "Transcription for batch ID: {batchId}, scan ID: {scanId} updated successfully",
                    batchId,
                    scanId
                );
        }

        public async Task ClearTranscriptionAsync(
            string userId,
            string batchId,
            string scanId,
            CancellationToken cancellationToken
        )
        {
            logger
                .LogInformation(
                    "Clearing transcription record for batch ID: {batchId}, scan ID: {scanId}",
                    batchId,
                    scanId
                );

            await
                batchRepository
                    .ClearBatchScanTranscriptionsAsync(
                        userId,
                        batchId,
                        scanId,
                        cancellationToken
                    );
            
            logger
                .LogInformation(
                    "Transcription for batch ID: {batchId}, scan ID: {scanId} cleared successfully",
                    batchId,
                    scanId
                );
        }
    }
}