using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Rocket.Domain.Batches;
using Rocket.Domain.Enum;
using Rocket.Domain.Exceptions;
using Rocket.Domain.ScannedImages;
using Rocket.Interfaces;

namespace Rocket.Infrastructure
{
    public class BatchHandler(
        ILogger<BatchHandler> logger,
        IBlobStore blobStore,
        ISha256Calculator sha256Calculator,
        IScannedImageRepository scannedImageRepository,
        IBatchRepository batchRepository,
        IExecutionRepository executionRepository,
        IThumbnailer thumbnailer
    ) : IBatchHandler
    {
        public async Task<ScannedImage> WriteAsync(
            byte[] imageData,
            string contentType,
            string fileExtension,
            string userId,
            string vendor,
            string description,
            string batchId,
            CancellationToken cancellationToken
        )
        {
            logger
                .LogInformation("Writing image data to store and repository");

            try
            {
                if ((imageData ?? []).Length == 0)
                    throw new RocketException(
                        "No file data provided.",
                        ApiStatusCodeEnum.NoAttachmentsFound
                    );

                var scannedImage = new ScannedImage();

                var hashString =
                    sha256Calculator
                        .CalculateSha256HashAndFormat(imageData);

                var thumbnail =
                    await
                        thumbnailer
                            .GenerateBase64ThumbnailAsync(
                                imageData,
                                cancellationToken
                            );

                var blobId =
                    await
                        blobStore
                            .SaveImageAsync(
                                imageData,
                                fileExtension,
                                cancellationToken
                            );

                scannedImage.UserId = userId;
                scannedImage.BlobId = blobId;
                scannedImage.CaptureDate = DateTime.UtcNow;
                scannedImage.ContentType = contentType;
                scannedImage.FileExtension = fileExtension;
                scannedImage.Sha256 = hashString;
                scannedImage.ThumbnailBase64 = thumbnail;
                scannedImage.Archived = false;
                scannedImage.Vendor = vendor;
                scannedImage.Description = description;

                var result =
                    await
                        batchRepository
                            .InsertBatchScanAsync(
                                scannedImage,
                                userId,
                                batchId,
                                cancellationToken
                            );

                logger
                    .LogInformation(
                        "Successfully saved scanned image for user ID: {userId}, batch ID: {batchId}",
                        userId,
                        batchId
                    );

                return result;
            }
            catch (Exception ex)
            {
                logger
                    .LogError(
                        "There was an error handling the scanned image: {error}",
                        ex.Message
                    );

                throw;
            }
        }

        public async Task<(ScannedImage record, byte[] imageData)> ReadAsync(
            string userId,
            string id,
            CancellationToken cancellationToken
        )
        {
            logger
                .LogInformation("Retrieving scan record and image data");

            try
            {
                var record =
                    await
                        scannedImageRepository
                            .GetScanByIdAsync(
                                userId,
                                id,
                                cancellationToken
                            );

                if (record == null)
                    throw new RocketException(
                        "The record was not found or you do not have access.",
                        ApiStatusCodeEnum.UnknownOrInaccessibleRecord
                    );

                byte[] imageData = [];
                if (!record.Archived)
                {
                    imageData =
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
                            id
                        );
                }
                else
                {
                    logger
                        .LogInformation(
                            "Record is archived, image data not loaded for user ID: {userId}, id: {id}",
                            userId,
                            id
                        );
                }

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

        public async Task<Batch> OpenAsync(
            string userId,
            CancellationToken cancellationToken
        )
        {
            logger
                .LogInformation(
                    "Opening new batch for user ID: {userId}",
                    userId
                );

            try
            {
                var batch =
                    new Batch
                    {
                        UserId = userId,
                        CaptureDate = DateTime.UtcNow,
                        Status = (int)BatchStatusEnum.Open,
                        Scans = []
                    };

                batch =
                    await
                        batchRepository
                            .InsertBatchAsync(
                                batch,
                                cancellationToken
                            );

                return batch;
            }
            catch (Exception ex)
            {
                logger
                    .LogError(
                        "There was an error opening a new batch: {error}",
                        ex.Message
                    );

                throw;
            }
        }

        public async Task<bool> ArchiveAsync(
            string userId,
            string id,
            CancellationToken cancellationToken
        )
        {
            logger
                .LogInformation("Archiving scan record and image data");

            try
            {
                var record =
                    await
                        scannedImageRepository
                            .GetScanByIdAsync(
                                userId,
                                id,
                                cancellationToken
                            );

                if (record == null)
                    throw new RocketException(
                        "The record was not found or you do not have access.",
                        ApiStatusCodeEnum.UnknownOrInaccessibleRecord
                    );

                if (record.Archived)
                    throw new RocketException(
                        "The record is already archived.",
                        ApiStatusCodeEnum.RecordIsArchived
                    );

                var deleted =
                    await
                        blobStore
                            .DeleteImageAsync(
                                record.BlobId,
                                record.FileExtension,
                                cancellationToken
                            );

                if (deleted)
                {
                    logger
                        .LogInformation(
                            "Successfully deleted image for user ID: {userId}, id: {id}",
                            userId,
                            id
                        );

                    await
                        scannedImageRepository
                            .ArchiveScanAsync(
                                userId,
                                id,
                                cancellationToken
                            );
                }
                else
                {
                    logger
                        .LogWarning(
                            "Image for user ID: {userId}, id: {id} was not deleted",
                            userId,
                            id
                        );
                }

                return deleted;
            }
            catch (Exception ex)
            {
                logger
                    .LogError(
                        "There was an error archiving the scanned image: {error}",
                        ex.Message
                    );

                throw;
            }
        }

        public async Task<bool> DeleteAsync(
            string userId,
            string id,
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
                            .GetBatchByIdAsync(
                                userId,
                                id,
                                cancellationToken
                            );

                if (record == null)
                    throw new RocketException(
                        "The record was not found or you do not have access.",
                        ApiStatusCodeEnum.UnknownOrInaccessibleRecord
                    );

                var allDeleted = true;
                foreach (var scan in record.Scans ?? [])
                {
                    try
                    {
                        allDeleted &=
                            await
                                blobStore
                                    .DeleteImageAsync(
                                        scan.BlobId,
                                        scan.FileExtension,
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
                                    scan.BlobId
                                );

                            allDeleted = false;
                        }
                        else throw;
                    }
                }

                if (allDeleted)
                {
                    logger
                        .LogInformation(
                            "Successfully deleted images for user ID: {userId}, batch ID: {id}",
                            userId,
                            id
                        );

                    var count =
                        await
                            executionRepository
                                .DeleteExecutionsByScanIdAsync(
                                    userId,
                                    id,
                                    cancellationToken
                                );

                    logger
                        .LogInformation(
                            "Successfully deleted {count} for user ID: {userId}, scan id: {id}",
                            count,
                            userId,
                            id
                        );

                    await
                        batchRepository
                            .DeleteBatchAsync(
                                userId,
                                id,
                                cancellationToken
                            );
                }
                else
                {
                    logger
                        .LogWarning(
                            "Image for user ID: {userId}, id: {id} was not deleted",
                            userId,
                            id
                        );
                }

                return allDeleted;
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

        public async Task CloseAsync(
            string userId,
            string batchId,
            CancellationToken cancellationToken
        )
        {
            logger
                .LogInformation(
                    "Closing batch for user ID: {userId}, batch ID: {batchId}",
                    userId,
                    batchId
                );

            try
            {
                await
                    batchRepository
                        .UpdateBatchFieldAsync(
                            userId,
                            batchId,
                            o => o.Status,
                            (int)BatchStatusEnum.Closed,
                            cancellationToken
                        );
            }
            catch (Exception ex)
            {
                logger
                    .LogError(
                        "There was an error closing batch ID: {batchId}, error: {error}",
                        batchId,
                        ex.Message
                    );

                throw;
            }
        }
    }
}