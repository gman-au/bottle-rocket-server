using System;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Rocket.Infrastructure.Detection.Extensions;
using Rocket.Interfaces;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace Rocket.Infrastructure.Detection
{
    public class SymbolDetector(
        IRocketbookPageTemplateRepository pageTemplateRepository,
        ILogger<SymbolDetector> logger
    ) : ISymbolDetector
    {
        private readonly PointF _lineStart = new(
            1.3f,
            0.275f
        ); // Adjust based on your layout

        private readonly PointF _lineEnd = new(
            8.5f,
            0.275f
        ); // Adjust based on your layout

        private const int NumSymbols = 7;


        public async Task<int[]> DetectSymbolMarksAsync(
            string qrCode,
            string qrCodeBoundingBox,
            byte[] imageBytes,
            CancellationToken cancellationToken
        )
        {
            logger
                .LogInformation("Rocketbook symbol detection requested");

            if ((imageBytes ?? []).Length == 0)
            {
                logger
                    .LogInformation("No image data found - skipping symbol detection");

                return null;
            }

            var matchingTemplate =
                await
                    pageTemplateRepository
                        .GetTemplateByQrCodeAsync(
                            qrCode,
                            cancellationToken
                        );

            if (matchingTemplate == null)
                return null;

            logger
                .LogInformation(
                    "Template match found for QR code: {qrCode}",
                    qrCode
                );

            try
            {
                using var inputStream =
                    new MemoryStream(imageBytes!);

                using var image =
                    await
                        Image
                            .LoadAsync<Rgba32>(
                                inputStream,
                                cancellationToken
                            );
                try
                {
                    // validate bounding box
                    var qrElements =
                        qrCodeBoundingBox
                            .Split(',')
                            .Select(float.Parse)
                            .ToArray();

                    if (qrElements.Length != 8)
                        throw new FormatException();

                    var qrPoints = new PointF[]
                    {
                        new(
                            qrElements[0],
                            qrElements[1]
                        ),
                        new(
                            qrElements[2],
                            qrElements[3]
                        ),
                        new(
                            qrElements[4],
                            qrElements[5]
                        ),
                        new(
                            qrElements[6],
                            qrElements[7]
                        )
                    };

                    var (lineStart, lineEnd) =
                        qrPoints
                            .GetDetectionLine(
                                _lineStart,
                                _lineEnd
                            );

                    var conceptualLineRegions =
                        new DetectionLineRegions(
                            lineStart,
                            lineEnd,
                            NumSymbols,
                            0.065f
                        );

                    var markedSymbols =
                        image
                            .DetectMarkedSymbols(conceptualLineRegions);

                    logger
                        .LogDebug(
                            "Dark pixel counts: {counts}",
                            string
                                .Join(
                                    "\r\n",
                                    conceptualLineRegions
                                        .Regions
                                        .Where(o => o.SymbolIndex.HasValue)
                                        .Select(o => $"region {o.SymbolIndex}: {o.DarkPixelCount}")
                                )
                        );

                    // Use the Mutate method to apply drawing operations
                    image
                        .Mutate(
                            ctx =>
                            {
                                ctx
                                    .DrawPolygon(
                                        Color.Red,
                                        2.0f,
                                        qrPoints
                                    );
                                ctx
                                    .DrawLine(
                                        Color.Blue,
                                        3.0f,
                                        lineStart,
                                        lineEnd
                                    );

                                foreach (var region in conceptualLineRegions.Regions)
                                {
                                    var markerColor = region.SymbolIndex.HasValue
                                        ? markedSymbols.Contains(region.SymbolIndex.Value) ? Color.Red : Color.Green
                                        : Color.Yellow;
                                    ctx
                                        .DrawLine(
                                            markerColor,
                                            3.0f,
                                            new PointF(
                                                region.Start,
                                                lineStart.Y
                                            ),
                                            new PointF(
                                                region.End,
                                                lineStart.Y
                                            )
                                        );
                                }
                            }
                        );

                    // Save the image
                    await
                        image
                            .SaveAsync(
                                "D:\\AZURE_STORAGE_TEST\\BOTTLE_ROCKET\\output_box_outline.png",
                                cancellationToken
                            );

                    if (markedSymbols.Length != 0)
                    {
                        logger
                            .LogInformation(
                            "Detected marks on symbols {symbolIndex}",
                            string.Join(",", markedSymbols)
                        );
                    }
                    else
                    {
                        logger
                            .LogInformation("No symbol marks detected");
                    }

                    return markedSymbols;
                }
                catch (FormatException)
                {
                    logger
                        .LogWarning(
                            "Malformed QR bounding box string: {boundingBox}",
                            qrCodeBoundingBox
                        );

                    throw;
                }
            }
            catch (Exception ex)
            {
                logger
                    .LogError(
                        ex,
                        "Could not process image"
                    );

                return [];
            }
        }
    }
}