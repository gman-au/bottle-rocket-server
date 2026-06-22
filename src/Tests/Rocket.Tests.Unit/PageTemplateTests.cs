using System.Collections.Generic;
using System.Linq;
using Rocket.Api.Host.Prepopulation;
using Rocket.Domain.PageTemplates;
using Xunit;

namespace Rocket.Tests.Unit
{
    public class PageTemplateTests
    {
        private readonly TestContext _context = new();

        [Theory]
        [InlineData("P01 V1J T02 S000")]
        [InlineData("P13 V0I S000000")]
        public void Test_Check_Template_Code(string expectedCode)
        {
            _context.ActGetAllTemplates();
            _context.AssertQrCodeTemplateExists(expectedCode);
        }

        private class TestContext
        {
            private IEnumerable<RocketbookPageTemplate> _results;

            public void ActGetAllTemplates()
            {
                _results = PageTemplates.GetRocketbookTemplates();
            }
            
            public void AssertQrCodeTemplateExists(string expectedCode)
            {
                var allCodes = _results.Select(o => o.QrCode) ?? [];
                Assert.Contains(expectedCode, allCodes);
            }
        }
    }
}