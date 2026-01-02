using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Rocket.Domain;
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
        public async Task HandleAsync(
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
    }
}