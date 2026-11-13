using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Rocket.Domain.Enum;
using Rocket.Domain.Exceptions;
using Rocket.Domain.ScannedImages;
using Rocket.Interfaces;

namespace Rocket.Infrastructure
{
    public class ScannedImageHandler(
        ILogger<ScannedImageHandler> logger,
        IBlobStore blobStore,
        ISha256Calculator sha256Calculator,
        IBatchRepository batchRepository
    ) : IScannedImageHandler
    {
        public async Task<(ScannedImage record, byte[] imageData)> ReadFromBatchAsync(
            string userId,
            string batchId,
            string scanId,
            CancellationToken cancellationToken
        )
        {
            logger
                .LogInformation("Retrieving scan record and image data");

            try
            {
                // TODO: check if batch is archived

                var record =
                    await
                        batchRepository
                            .GetScanByIdAsync(
                                userId,
                                batchId,
                                scanId,
                                cancellationToken
                            );

                if (record == null)
                    throw new RocketException(
                        "The record was not found or you do not have access.",
                        ApiStatusCodeEnum.UnknownOrInaccessibleRecord
                    );

                var imageData =
                    await
                        blobStore
                            .LoadImageAsync(
                                record.BlobId,
                                record.FileExtension,
                                cancellationToken
                            );

                var hashString =
                    sha256Calculator
                        .CalculateSha256HashAndFormat(imageData);

                if (hashString != record.Sha256)
                    throw new RocketException(
                        "The image data has been modified or corrupted since it was saved.",
                        ApiStatusCodeEnum.FileDataCorrupted
                    );

                logger
                    .LogInformation(
                        "Successfully retrieved scanned image for user ID: {userId}, id: {id}",
                        userId,
                        scanId
                    );

                return (record, imageData);
            }
            catch (Exception ex)
            {
                logger
                    .LogError(
                        "There was an error retrieving the scanned image: {error}",
                        ex.Message
                    );

                throw;
            }
        }

        public async Task<bool> DeleteFromBatchAsync(
            string userId,
            string batchId,
            string scanId,
            CancellationToken cancellationToken
        )
        {
            logger
                .LogInformation("Deleting scan record and image data");

            try
            {
                var record =
                    await
                        batchRepository
                            .GetScanByIdAsync(
                                userId,
                                batchId,
                                scanId,
                                cancellationToken
                            );

                if (record == null)
                    throw new RocketException(
                        "The record was not found or you do not have access.",
                        ApiStatusCodeEnum.UnknownOrInaccessibleRecord
                    );

                var deleted = false;
                try
                {
                    deleted =
                        await
                            blobStore
                                .DeleteImageAsync(
                                    record.BlobId,
                                    record.FileExtension,
                                    cancellationToken
                                );
                }
                catch (RocketException ex)
                {
                    if (ex.ApiStatusCode == (int)ApiStatusCodeEnum.UnknownOrInaccessibleRecord)
                    {
                        logger
                            .LogWarning(
                                "Image with user ID: {userId}, blob ID: {blobId} could not be found for deletion, continuing",
                                userId,
                                record.BlobId
                            );

                        deleted = true;
                    }
                    else throw;
                }

                if (deleted)
                {
                    logger
                        .LogInformation(
                            "Successfully deleted image for user ID: {userId}, id: {id}",
                            userId,
                            scanId
                        );

                    await
                        batchRepository
                            .DeleteScanByIdAsync(
                                userId,
                                batchId,
                                scanId,
                                cancellationToken
                            );
                }
                else
                {
                    logger
                        .LogWarning(
                            "Image for user ID: {userId}, id: {id} was not deleted",
                            userId,
                            scanId
                        );
                }

                return deleted;
            }
            catch (Exception ex)
            {
                logger
                    .LogError(
                        "There was an error deleting the scanned image: {error}",
                        ex.Message
                    );

                throw;
            }
        }
    }
}