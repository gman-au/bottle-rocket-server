using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Rocket.Domain.ScannedImages;
using Rocket.Interfaces;

namespace Rocket.Infrastructure
{
    public class Transcriber(
        ILogger<Transcriber> logger,
        IBatchRepository batchRepository
    ) : ITranscriber
    {
        public async Task ApplyTranscriptionAsync(
            string userId,
            string batchId,
            string scanId,
            string executionId,
            string executionName,
            DateTime executionDate,
            string text,
            CancellationToken cancellationToken
        )
        {
            logger
                .LogInformation(
                    "Updating transcription record for batch ID: {batchId}, scan ID: {scanId}",
                    batchId,
                    scanId
                );

            var transcription =
                new Transcription
                {
                    ExecutionId = executionId,
                    ExecutionName = executionName,
                    TranscriptionDate = executionDate,
                    Text = text
                };

            await
                batchRepository
                    .UpdateBatchScanTranscriptionAsync(
                        userId,
                        batchId,
                        scanId,
                        transcription,
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