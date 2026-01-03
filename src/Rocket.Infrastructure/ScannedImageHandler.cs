using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Rocket.Domain;
using Rocket.Domain.Enum;
using Rocket.Domain.Exceptions;
using Rocket.Interfaces;

namespace Rocket.Infrastructure
{
    public class ScannedImageHandler(
        ILogger<ScannedImageHandler> logger,
        IBlobStore blobStore,
        ISha256Calculator sha256Calculator,
        IScannedImageRepository scannedImageRepository,
        IThumbnailer thumbnailer
    ) : IScannedImageHandler
    {
        public async Task WriteAsync(
            byte[] imageData,
            string contentType,
            string fileExtension,
            string userId,
            CancellationToken cancellationToken
        )
        {
            logger
                .LogInformation("Writing image data to store and repository");

            try
            {
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

                await
                    scannedImageRepository
                        .SaveCaptureAsync(
                            scannedImage,
                            cancellationToken
                        );

                logger
                    .LogInformation(
                        "Successfully saved scanned image for user ID: {userId}",
                        userId
                    );
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
                            .FetchScanAsync(
                                userId,
                                id,
                                cancellationToken
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
                        id
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
    }
}