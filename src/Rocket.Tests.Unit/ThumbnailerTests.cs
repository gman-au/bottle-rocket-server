using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.Extensions.Logging;
using Rocket.Infrastructure.Thumbnails;
using Rocket.Tests.Unit.Extensions;
using Xunit;

namespace Rocket.Tests.Unit
{
    public class ThumbnailerTests
    {
        private readonly TestContext _context = new();

        [Fact]
        public async Task Test_Build_Valid_Thumbnail()
        {
            _context.ArrangeValidDataToGenerate();
            await _context.ActBuildThumbnail();
            _context.AssertNonEmptyValue();
        }

        [Fact]
        public async Task Test_Build_Empty_Thumbnail()
        {
            _context.ArrangeEmptyDataToGenerate();
            await _context.ActBuildThumbnail();
            _context.AssertEmptyValue();
        }

        [Fact]
        public async Task Test_Build_Unknown_Thumbnail()
        {
            _context.ArrangeUnknownDataToGenerate();
            await _context.ActBuildThumbnail();
            _context.AssertEmptyValue();
        }

        private class TestContext
        {
            private readonly Thumbnailer _sut;
            private byte[] _imageData;
            private string _result;

            public TestContext()
            {
                var fixture =
                    FixtureEx
                        .CreateNSubstituteFixture();

                _sut =
                    new Thumbnailer(
                        fixture
                            .Freeze<ILogger<Thumbnailer>>()
                    );
            }

            public void ArrangeValidDataToGenerate()
            {
                _imageData =
                [
                    0x89, 0x50, 0x4e, 0x47, 0x0d, 0x0a, 0x1a, 0x0a, 0x00, 0x00, 0x00,
                    0x0d, 0x49, 0x48, 0x44, 0x52, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00,
                    0x00, 0x01, 0x01, 0x00, 0x00, 0x00, 0x00, 0x37, 0x6e, 0xf9, 0x24,
                    0x00, 0x00, 0x00, 0x0a, 0x49, 0x44, 0x41, 0x54, 0x78, 0x01, 0x63,
                    0x60, 0x00, 0x00, 0x00, 0x02, 0x00, 0x01, 0x73, 0x75, 0x01, 0x18,
                    0x00, 0x00, 0x00, 0x00, 0x49, 0x45, 0x4e, 0x44, 0xae, 0x42, 0x60,
                    0x82
                ];
            }


            public void ArrangeUnknownDataToGenerate()
            {
                _imageData =
                [
                    0x19, 0x51
                ];
            }


            public void ArrangeEmptyDataToGenerate()
            {
                _imageData = [];
            }

            public async Task ActBuildThumbnail()
            {
                _result =
                    await
                        _sut
                            .GenerateBase64ThumbnailAsync(
                                _imageData,
                                CancellationToken.None
                            );
            }

            public void AssertNonEmptyValue()
            {
                Assert
                    .NotEmpty(_result);
            }

            public void AssertEmptyValue()
            {
                Assert
                    .Empty(_result);
            }
        }
    }
}