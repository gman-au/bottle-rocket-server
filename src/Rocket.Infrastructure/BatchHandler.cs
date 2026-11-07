using System;
using System.Collections.Generic;
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

        public async Task<Dictionary<ScannedImage, byte[]>> ReadAsync(
            string userId,
            string batchId,
            CancellationToken cancellationToken
        )
        {
            logger
                .LogInformation("Retrieving batch record and image data");

            try
            {
                var record =
                    await
                        batchRepository
                            .GetBatchByIdAsync(
                                userId,
                                batchId,
                                cancellationToken
                            );

                if (record == null)
                    throw new RocketException(
                        "The record was not found or you do not have access.",
                        ApiStatusCodeEnum.UnknownOrInaccessibleRecord
                    );

                if (record.Archived)
                    throw new RocketException(
                        "Record is archived and cannot be processed.",
                        ApiStatusCodeEnum.RecordIsArchived
                    );

                var results = new Dictionary<ScannedImage, byte[]>();

                if (!record.Archived)
                {
                    foreach (var scan in record.Scans ?? [])
                    {
                        var imageData =
                            await
                                blobStore
                                    .LoadImageAsync(
                                        scan.BlobId,
                                        scan.FileExtension,
                                        cancellationToken
                                    );

                        var hashString =
                            sha256Calculator
                                .CalculateSha256HashAndFormat(imageData);

                        if (hashString != scan.Sha256)
                            throw new RocketException(
                                $"The image data for scan ID {scan.Id} has been modified or corrupted since it was saved.",
                                ApiStatusCodeEnum.FileDataCorrupted
                            );

                        logger
                            .LogInformation(
                                "Successfully retrieved scanned image for user ID: {userId}, scan ID: {scanId}",
                                userId,
                                scan.Id
                            );
                        
                        results
                            .Add(scan, imageData);
                    }
                }
                else
                {
                    logger
                        .LogInformation(
                            "Record is archived, image data not loaded for user ID: {userId}, batch ID: {batchId}",
                            userId,
                            batchId
                        );
                }

                return results;
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

        public async Task<bool> ArchiveAsync(
            string userId,
            string batchId,
            CancellationToken cancellationToken
        )
        {
            logger
                .LogInformation("Archiving batch");

            try
            {
                var record =
                    await
                        batchRepository
                            .GetBatchByIdAsync(
                                userId,
                                batchId,
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

                var allDeleted =
                    await
                        DeleteScanFilesAsync(
                            record.Scans,
                            cancellationToken
                        );

                if (allDeleted)
                {
                    logger
                        .LogInformation(
                            "Successfully deleted images for user ID: {userId}, batch ID: {batchId}",
                            userId,
                            batchId
                        );

                    await
                        batchRepository
                            .ArchiveBatchAsync(
                                userId,
                                batchId,
                                cancellationToken
                            );
                }
                else
                {
                    logger
                        .LogWarning(
                            "Not all scans for user ID: {userId}, batch ID: {batchId} were deleted",
                            userId,
                            batchId
                        );
                }

                return allDeleted;
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
            string batchId,
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
                                batchId,
                                cancellationToken
                            );

                if (record == null)
                    throw new RocketException(
                        "The record was not found or you do not have access.",
                        ApiStatusCodeEnum.UnknownOrInaccessibleRecord
                    );

                var allDeleted =
                    await
                        DeleteScanFilesAsync(
                            record.Scans,
                            cancellationToken
                        );

                if (allDeleted)
                {
                    logger
                        .LogInformation(
                            "Successfully deleted images for user ID: {userId}, batch ID: {id}",
                            userId,
                            batchId
                        );

                    var count =
                        await
                            executionRepository
                                .DeleteExecutionsByBatchIdAsync(
                                    userId,
                                    batchId,
                                    cancellationToken
                                );

                    logger
                        .LogInformation(
                            "Successfully deleted {count} for user ID: {userId}, scan id: {id}",
                            count,
                            userId,
                            batchId
                        );

                    await
                        batchRepository
                            .DeleteBatchAsync(
                                userId,
                                batchId,
                                cancellationToken
                            );
                }
                else
                {
                    logger
                        .LogWarning(
                            "Image for user ID: {userId}, id: {id} was not deleted",
                            userId,
                            batchId
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

        private async Task<bool> DeleteScanFilesAsync(
            IEnumerable<ScannedImage> scans,
            CancellationToken cancellationToken
        )
        {
            var allDeleted = true;
            foreach (var scan in scans ?? [])
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
                                "Image with blob ID: {blobId} could not be found for deletion, continuing",
                                scan.BlobId
                            );

                        allDeleted = false;
                    }
                    else throw;
                }
            }

            return allDeleted;
        }
    }
}