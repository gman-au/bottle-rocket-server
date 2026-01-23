using System.Collections.Generic;
using System.Linq;
using Rocket.Infrastructure.Detection.Extensions;
using SixLabors.ImageSharp;

namespace Rocket.Infrastructure.Detection
{
    public class DetectionLineRegions
    {
        public float Distance { get; set; }

        public IEnumerable<LineRegion> Regions { get; set; }

        public PointF LineStart { get; set; }

        public PointF LineEnd { get; set; }

        public LineRegion GetAffectedRegion(float x)
        {
            return
                Regions
                    .FirstOrDefault(o => o.End > x && o.Start <= x);
        }

        public DetectionLineRegions(
            PointF lineStart,
            PointF lineEnd,
            int numberOfSymbols,
            float relativeSymbolRegionSize
        )
        {
            LineStart = lineStart;
            LineEnd = lineEnd;

            var distance =
                LineEx
                    .Distance(
                        lineStart,
                        lineEnd
                    );
            
            var singleRegionSize = distance * relativeSymbolRegionSize;
            var totalRegionSize = singleRegionSize * numberOfSymbols;
            var totalNonRegionSize = distance - totalRegionSize;
            var nonRegionSize = totalNonRegionSize / (numberOfSymbols - 1);
            var segmentSize = singleRegionSize + nonRegionSize;

            // first region is symbol region of dist A
            // second region is non-symbol region of dist B
            // there should be (symbols - 1) of these, plus one final symbol region dist A
            // i.e. four symbols => A B A B A B A

            var regionList = new List<LineRegion>();

            for (var i = 0; i < numberOfSymbols; i++)
            {
                regionList
                    .Add(
                        new LineRegion
                        {
                            SymbolIndex = i,
                            Start = i * segmentSize + lineStart.X,
                            End = i * segmentSize + singleRegionSize + lineStart.X
                        }
                    );

                if (i < numberOfSymbols - 1)
                {
                    regionList
                        .Add(
                            new LineRegion
                            {
                                SymbolIndex = null,
                                Start = i * segmentSize + singleRegionSize + lineStart.X,
                                End = i * segmentSize + singleRegionSize + nonRegionSize + lineStart.X
                            }
                        );
                }
            }

            Regions =
                regionList
                    .ToArray();
        }
    }
}