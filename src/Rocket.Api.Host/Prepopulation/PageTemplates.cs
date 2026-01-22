using System;
using System.Collections.Generic;
using Rocket.Domain.Enum;
using Rocket.Domain.PageTemplates;

namespace Rocket.Api.Host.Prepopulation
{
    internal static class PageTemplates
    {
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
                        yield return
                            new RocketbookPageTemplate
                            {
                                QrCode = qrCode,
                                PaperSizeType = (int)PaperSizeTypeEnum.A4,
                                QrCodeOrientationType = (int)qrCodeOrientationType,
                                RocketbookPageTemplateType = (int)rocketbookPageTemplateType,
                                SymbolsBoundingBox = null
                            };
                    }
                }
            }
        }
    }
}