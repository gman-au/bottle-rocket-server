using System.Linq;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Rocket.Infrastructure.Detection.Extensions
{
    internal static class ImageEx
    {
        private const int BrightnessThreshold = 180; // Adjust for mark sensitivity
        private const int DarknessThreshold = 2;
        private const int SampleSize = 800;
        
        public static int[] DetectMarkedSymbols(
            this Image<Rgba32> image,
            DetectionLineRegions detectionLineRegions
        )
        {
            var lineStart = detectionLineRegions.LineStart;
            var lineEnd = detectionLineRegions.LineEnd;
            
            // Sample pixels along the line
            for (var i = 0; i < SampleSize; i++)
            {
                var t = i / (float)(SampleSize - 1);
                
                var x = (int)(lineStart.X + t * (lineEnd.X - lineStart.X));
                var y = (int)(lineStart.Y + t * (lineEnd.Y - lineStart.Y));
                
                if (x >= 0 && x < image.Width && y >= 0 && y < image.Height)
                {
                    var pixel = image[x, y];
            
                    if (!IsDark(pixel)) continue;
                    
                    var affectedRegion = 
                        detectionLineRegions
                            .GetAffectedRegion(x);

                    if (affectedRegion == null) continue;
                    
                    affectedRegion.DarkPixelCount++;
                }
            }

            var markedRegions =
                detectionLineRegions
                    .Regions
                    .Where(
                        o =>
                            o.DarkPixelCount > DarknessThreshold &&
                            o.SymbolIndex.HasValue
                    )
                    .Select(o => o.SymbolIndex.Value);

            return 
                markedRegions
                    .ToArray();
        }

        private static bool IsDark(Rgba32 pixel)
        {
            // Calculate brightness
            var brightness = (pixel.R + pixel.G + pixel.B) / 3;
            return brightness < BrightnessThreshold;
        }
    }
}