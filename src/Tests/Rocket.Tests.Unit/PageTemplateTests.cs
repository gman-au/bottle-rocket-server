using System.Collections.Generic;
using System.Linq;
using Rocket.Api.Host.Prepopulation;
using Rocket.Domain.Enum;
using Rocket.Domain.PageTemplates;
using Xunit;

namespace Rocket.Tests.Unit
{
    public class PageTemplateTests
    {
        private readonly TestContext _context = new();

        [Theory]
        [InlineData("P01 V1J T02 S000", QrCodeOrientationTypeEnum.BottomLeft)]
        [InlineData("P13 V0I S000000", QrCodeOrientationTypeEnum.BottomRight)]
        [InlineData("P24 V0I S000000", QrCodeOrientationTypeEnum.BottomLeft)]
        public void Test_Check_Template_Code(string expectedCode, QrCodeOrientationTypeEnum expectedOrientation)
        {
            _context.ActGetAllTemplates();
            _context.AssertQrCodeTemplateExists(expectedCode, expectedOrientation);
        }

        private class TestContext
        {
            private IEnumerable<RocketbookPageTemplate> _results;

            public void ActGetAllTemplates()
            {
                _results = 
                    PageTemplates
                        .GetRocketbookTemplates();
            }
            
            public void AssertQrCodeTemplateExists(string expectedCode, QrCodeOrientationTypeEnum expectedOrientation)
            {
                var allCodes =
                    (_results
                        .Select(o => o.QrCode) ?? [])
                    .ToList();
                
                Assert
                    .Contains(expectedCode, allCodes);
                
                var template = 
                    _results
                        .First(o => o.QrCode == expectedCode);
                
                Assert
                    .Equal((int)expectedOrientation, template.QrCodeOrientationType);
            }
        }
    }
}