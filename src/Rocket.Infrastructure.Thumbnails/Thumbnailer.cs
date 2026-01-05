using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Rocket.Interfaces;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

namespace Rocket.Infrastructure.Thumbnails
{
    public class Thumbnailer(ILogger<Thumbnailer> logger) : IThumbnailer
    {
        private const int MaxThumbnailSize = 200;

        public async Task<string> GenerateBase64ThumbnailAsync(
            byte[] imageBytes,
            CancellationToken cancellationToken
        )
        {
            if ((imageBytes ?? []).Length == 0)
                return
                    string
                        .Empty;

            try
            {
                using var inputStream =
                    new MemoryStream(imageBytes!);

                using var image =
                    await
                        Image
                            .LoadAsync(
                                inputStream,
                                cancellationToken
                            );

                // Resize the image while maintaining aspect ratio
                image
                    .Mutate(ctx =>
                        ctx
                            .Resize(
                                new ResizeOptions
                                {
                                    Size =
                                        new Size(
                                            MaxThumbnailSize,
                                            MaxThumbnailSize
                                        ),
                                    Mode =
                                        ResizeMode
                                            .Max // Ensures image fits within the bounds without stretching
                                }
                            )
                    );

                var outputStream = new MemoryStream();

                var jpegEncoder = new JpegEncoder { Quality = 75 };

                await
                    image
                        .SaveAsync(
                            outputStream,
                            jpegEncoder,
                            cancellationToken
                        );

                outputStream.Position = 0;

                var byteArray =
                    outputStream
                        .ToArray();

                return
                    Convert
                        .ToBase64String(byteArray);
            }
            catch (UnknownImageFormatException)
            {
                logger
                    .LogWarning("Could not generate thumbnail - unrecognized image format");

                return
                    string
                        .Empty;
            }
        }
    }
}