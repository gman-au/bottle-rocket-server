using System;
using System.Linq;
using System.Numerics;
using SixLabors.ImageSharp;

namespace Rocket.Infrastructure.Detection.Extensions
{
    internal static class LineEx
    {
        public static (PointF start, PointF end) GetDetectionLine(
            this PointF[] qrCorners,
            PointF templateLineStart,
            PointF templateLineEnd
        )
        {
            var topLeft = qrCorners[0];
            var topRight = qrCorners[1];
            var bottomLeft = qrCorners[3];

            var qrCenter =
                new PointF(
                    qrCorners.Average(p => p.X),
                    qrCorners.Average(p => p.Y)
                );

            var qrWidth =
                Distance(
                    topLeft,
                    topRight
                );
            var qrHeight =
                Distance(
                    topLeft,
                    bottomLeft
                );
            var qrAngle =
                MathF
                    .Atan2(
                        topRight.Y - topLeft.Y,
                        topRight.X - topLeft.X
                    );

            var transform =
                Matrix3x2
                    .CreateScale(
                        qrWidth,
                        qrHeight
                    ) *
                Matrix3x2
                    .CreateRotation(qrAngle) *
                Matrix3x2
                    .CreateTranslation(
                        qrCenter.X,
                        qrCenter.Y
                    );

            var start =
                PointF
                    .Transform(
                        templateLineStart,
                        transform
                    );
            var end =
                PointF
                    .Transform(
                        templateLineEnd,
                        transform
                    );

            return (start, end);
        }

        public static float Distance(PointF p1, PointF p2)
        {
            var dx = p2.X - p1.X;
            var dy = p2.Y - p1.Y;

            return
                MathF
                    .Sqrt(dx * dx + dy * dy);
        }
    }
}