using System;
using System.Collections.Generic;
using Rocket.Api.Host.Extensions;
using Rocket.Domain.Enum;
using Rocket.Domain.PageTemplates;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Rocket.Tests.Unit")]

namespace Rocket.Api.Host.Prepopulation
{
    internal static class PageTemplates
    {
        private const string BottomLeftDetectionLineTransform = "1.3,0.275,8.5,0.275";
        private const string BottomRightDetectionLineTransform = "-7.75,0.275,-0.85,0.275";

        private static readonly Dictionary<int, string> DetectionLines = new()
        {
            { (int)QrCodeOrientationTypeEnum.BottomLeft, BottomLeftDetectionLineTransform },
            { (int)QrCodeOrientationTypeEnum.BottomRight, BottomRightDetectionLineTransform }
        };

        public static IEnumerable<RocketbookPageTemplate> GetRocketbookTemplates()
        {
            // Modern products
            foreach (var qrCodeOrientationType in Enum.GetValues<QrCodeOrientationTypeEnum>())
            {
                var pCode = $"P{((int)qrCodeOrientationType).ToString().PadLeft(2, '0')}";

                foreach (var productCode in Enum.GetValues<RocketbookModernProductTypeEnum>())
                {
                    var vCode = $"V{productCode.ToCode()}";

                    foreach (var rocketbookPageTemplateType in Enum.GetValues<RocketbookPageTemplateTypeEnum>())
                    {
                        var tCode = $"T{((int)rocketbookPageTemplateType).ToString().PadLeft(2, '0')}";

                        var qrCode = $"{pCode} {vCode} {tCode} S000";

                        DetectionLines.TryGetValue(
                            (int)qrCodeOrientationType,
                            out var symbolsDetectionLine
                        );

                        yield return
                            new RocketbookPageTemplate
                            {
                                QrCode = qrCode,
                                PaperSizeType = (int)PaperSizeTypeEnum.A4,
                                QrCodeOrientationType = (int)qrCodeOrientationType,
                                RocketbookPageTemplateType = (int)rocketbookPageTemplateType,
                                SymbolsDetectionLine = symbolsDetectionLine
                            };
                    }
                }
            }

            // Legacy products
            foreach (var productCode in Enum.GetValues<RocketbookLegacyProductTypeEnum>())
            {
                var vCode = $"V{productCode.ToCode()}";

                for (var pageNumber = 1; pageNumber <= 36; pageNumber++)
                {
                    var pCode = $"P{pageNumber.ToString().PadLeft(2, '0')}";
                    
                    var qrCode = $"{pCode} {vCode} S0000000";

                    var qrCodeOrientationType = (QrCodeOrientationTypeEnum)(pageNumber % 2 + 1);
                    
                    DetectionLines.TryGetValue(
                        (int)qrCodeOrientationType,
                        out var symbolsDetectionLine
                    );
                    
                    yield return
                        new RocketbookPageTemplate
                        {
                            QrCode = qrCode,
                            PaperSizeType = (int)PaperSizeTypeEnum.A5,
                            QrCodeOrientationType = (int)qrCodeOrientationType,
                            RocketbookPageTemplateType = (int)RocketbookPageTemplateTypeEnum.NotSet,
                            SymbolsDetectionLine = symbolsDetectionLine
                        };
                }
            }
        }
    }
}