using System;
using System.Collections.Generic;
using Rocket.Domain.Core.Enum;
using Rocket.Domain.PageTemplates;

namespace Rocket.Api.Host.Prepopulation
{
    internal static class PageTemplates
    {
        private const string BottomLeftDetectionLineTransform  = "1.3,0.275,8.5,0.275";
        private const string BottomRightDetectionLineTransform  = "-7.75,0.275,-0.85,0.275";

        private static readonly Dictionary<int, string> DetectionLines = new()
        {
            { (int)QrCodeOrientationTypeEnum.BottomLeft, BottomLeftDetectionLineTransform },
            { (int)QrCodeOrientationTypeEnum.BottomRight, BottomRightDetectionLineTransform }
        };

        public static IEnumerable<RocketbookPageTemplate> GetRocketbookTemplates()
        {
            foreach (var qrCodeOrientationType in Enum.GetValues<QrCodeOrientationTypeEnum>())
            {
                var pCode = $"P{((int)qrCodeOrientationType).ToString().PadLeft(2, '0')}";

                foreach (var productCode in Enum.GetValues<RocketbookProductTypeEnum>())
                {
                    var vCode = $"V{(int)productCode:X2}";

                    foreach (var rocketbookPageTemplateType in Enum.GetValues<RocketbookPageTemplateTypeEnum>())
                    {
                        var tCode = $"T{((int)rocketbookPageTemplateType).ToString().PadLeft(2, '0')}";

                        var qrCode = $"{pCode} {vCode} {tCode} S000";

                        DetectionLines.TryGetValue((int)qrCodeOrientationType, out var symbolsDetectionLine);
                        
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
        }
    }
}