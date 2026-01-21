using System;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Rocket.Domain;
using Rocket.Domain.Enum;
using Rocket.Domain.Exceptions;
using Rocket.Infrastructure;
using Rocket.Interfaces;
using Rocket.Tests.Unit.Extensions;
using Xunit;

namespace Rocket.Tests.Unit
{
    public class ScannedImageHandlerTests
    {
        private readonly TestContext _context = new();

        [Fact]
        public async Task Test_Save_Valid_Data()
        {
            _context.ArrangeValidDataToSave();
            await _context.ActWriteScanAsync();
            _context.AssertShaCalculatorWasCalledOnce();
            _context.AssertThumbnailerWasCalledOnce();
            _context.AssertBlobSaveWasCalledOnce();
            _context.AssertImageRepositorySaveWasCalledOnce();
        }

        [Fact]
        public async Task Test_Save_Empty_Data()
        {
            _context.ArrangeEmptyDataToSave();
            await _context.AssertWriteScanThrowsNoAttachmentErrorAsync();
            _context.AssertShaCalculatorWasNotCalled();
            _context.AssertThumbnailerWasNotCalled();
            _context.AssertBlobSaveWasNotCalled();
            _context.AssertImageRepositorySaveWasNotCalled();
        }

        [Fact]
        public async Task Test_Load_Valid_Data()
        {
            _context.ArrangeValidDataToLoad();
            _context.ArrangeMatchingSha();
            await _context.ActReadScanAsync();
            _context.AssertImageRepositoryLoadWasCalledOnce();
            _context.AssertShaCalculatorWasCalledOnce();
            _context.AssertBlobLoadWasCalledOnce();
        }

        [Fact]
        public async Task Test_Load_Corrupted_Data()
        {
            _context.ArrangeValidDataToLoad();
            _context.ArrangeMismatchingSha();
            await _context.AssertReadScanThrowsCorruptedDataErrorAsync();
            _context.AssertImageRepositoryLoadWasCalledOnce();
            _context.AssertShaCalculatorWasCalledOnce();
            _context.AssertBlobLoadWasCalledOnce();
        }

        [Fact]
        public async Task Test_Load_No_Record_Data()
        {
            _context.ArrangeNoRecordFoundToLoad();
            await _context.AssertReadScanThrowsNoAttachmentErrorAsync();
            _context.AssertImageRepositoryLoadWasCalledOnce();
            _context.AssertShaCalculatorWasNotCalled();
            _context.AssertBlobLoadWasNotCalled();
        }

        private class TestContext
        {
            private readonly ScannedImageHandler _sut;
            private byte[] _imageData;
            private string _userId;
            private string _contentType;
            private string _fileExtension;
            private readonly IBlobStore _blobStore;
            private readonly ISha256Calculator _sha256Calculator;
            private readonly IScannedImageRepository _scannedImageRepository;
            private readonly IThumbnailer _thumbnailer;
            private readonly IFixture _fixture;
            private string _qrCode;
            private string _qrBoundingBox;

            public TestContext()
            {
                _fixture = FixtureEx
                    .CreateNSubstituteFixture();

                _blobStore = _fixture.Freeze<IBlobStore>();
                _sha256Calculator = _fixture.Freeze<ISha256Calculator>();
                _scannedImageRepository = _fixture.Freeze<IScannedImageRepository>();
                _thumbnailer = _fixture.Freeze<IThumbnailer>();

                _sut =
                    new ScannedImageHandler(
                        _fixture.Freeze<ILogger<ScannedImageHandler>>(),
                        _blobStore,
                        _sha256Calculator,
                        _scannedImageRepository,
                        _thumbnailer
                    );
            }

            public void ArrangeValidDataToSave()
            {
                _imageData = [0x12];
                _userId = Guid.NewGuid().ToString();
                _contentType = "image/jpeg";
                _fileExtension = ".jpg";
                _qrCode = "1234";
                _qrBoundingBox = "1234";
                _fileExtension = ".jpg";
            }

            public void ArrangeEmptyDataToSave()
            {
                _imageData = [];
                _userId = Guid.NewGuid().ToString();
                _contentType = "image/jpeg";
                _fileExtension = ".jpg";
            }

            public void ArrangeValidDataToLoad()
            {
                _scannedImageRepository
                    .GetScanByIdAsync(null, null, CancellationToken.None)
                    .ReturnsForAnyArgs(
                        Task
                            .FromResult(
                                _fixture
                                    .Build<ScannedImage>()
                                    .With(o => o.Sha256, "abc123")
                                    .Create())
                        );
            }

            public void ArrangeMatchingSha()
            {
                _sha256Calculator
                    .CalculateSha256HashAndFormat(null)
                    .ReturnsForAnyArgs("abc123");
            }

            public void ArrangeMismatchingSha()
            {
                _sha256Calculator
                    .CalculateSha256HashAndFormat(null)
                    .ReturnsForAnyArgs("def456");
            }

            public void ArrangeNoRecordFoundToLoad()
            {
                _scannedImageRepository
                    .GetScanByIdAsync(null, null, CancellationToken.None)
                    .ReturnsForAnyArgs(Task.FromResult<ScannedImage>(null));
            }

            public async Task ActWriteScanAsync()
            {
                await
                    _sut
                        .WriteAsync(
                            _imageData,
                            _contentType,
                            _fileExtension,
                            _userId,
                            _qrCode,
                            _qrBoundingBox,
                            CancellationToken.None
                        );
            }

            public async Task ActReadScanAsync()
            {
                (_, _imageData) =
                    await
                        _sut
                            .ReadAsync(
                                null,
                                null,
                                CancellationToken.None
                            );
            }

            public async Task AssertWriteScanThrowsNoAttachmentErrorAsync()
            {
                var exception =
                    await
                        Assert
                            .ThrowsAsync<RocketException>(async () => await ActWriteScanAsync());

                Assert
                    .Equal((int)ApiStatusCodeEnum.NoAttachmentsFound, exception.ApiStatusCode);
            }

            public async Task AssertReadScanThrowsNoAttachmentErrorAsync()
            {
                var exception =
                    await
                        Assert
                            .ThrowsAsync<RocketException>(async () => await ActReadScanAsync());

                Assert
                    .Equal((int)ApiStatusCodeEnum.UnknownOrInaccessibleRecord, exception.ApiStatusCode);
            }

            public async Task AssertReadScanThrowsCorruptedDataErrorAsync()
            {
                var exception =
                    await
                        Assert
                            .ThrowsAsync<RocketException>(async () => await ActReadScanAsync());

                Assert
                    .Equal((int)ApiStatusCodeEnum.FileDataCorrupted, exception.ApiStatusCode);
            }

            public void AssertBlobSaveWasNotCalled()
            {
                _blobStore
                    .ReceivedWithAnyArgs(0)
                    .SaveImageAsync(null, null, CancellationToken.None);
            }

            public void AssertBlobSaveWasCalledOnce()
            {
                _blobStore
                    .ReceivedWithAnyArgs(1)
                    .SaveImageAsync(null, null, CancellationToken.None);
            }

            public void AssertBlobLoadWasNotCalled()
            {
                _blobStore
                    .ReceivedWithAnyArgs(0)
                    .LoadImageAsync(null, null, CancellationToken.None);
            }

            public void AssertBlobLoadWasCalledOnce()
            {
                _blobStore
                    .ReceivedWithAnyArgs(1)
                    .LoadImageAsync(null, null, CancellationToken.None);
            }

            public void AssertShaCalculatorWasNotCalled()
            {
                _sha256Calculator
                    .ReceivedWithAnyArgs(0)
                    .CalculateSha256HashAndFormat(null);
            }

            public void AssertShaCalculatorWasCalledOnce()
            {
                _sha256Calculator
                    .ReceivedWithAnyArgs(1)
                    .CalculateSha256HashAndFormat(null);
            }

            public void AssertThumbnailerWasNotCalled()
            {
                _thumbnailer
                    .ReceivedWithAnyArgs(0)
                    .GenerateBase64ThumbnailAsync(null, CancellationToken.None);
            }

            public void AssertThumbnailerWasCalledOnce()
            {
                _thumbnailer
                    .ReceivedWithAnyArgs(1)
                    .GenerateBase64ThumbnailAsync(null, CancellationToken.None);
            }

            public void AssertImageRepositoryLoadWasNotCalled()
            {
                _scannedImageRepository
                    .ReceivedWithAnyArgs(0)
                    .GetScanByIdAsync(null, null, CancellationToken.None);
            }

            public void AssertImageRepositoryLoadWasCalledOnce()
            {
                _scannedImageRepository
                    .ReceivedWithAnyArgs(1)
                    .GetScanByIdAsync(null, null, CancellationToken.None);
            }

            public void AssertImageRepositorySaveWasNotCalled()
            {
                _scannedImageRepository
                    .ReceivedWithAnyArgs(0)
                    .InsertScanAsync(null, CancellationToken.None);
            }

            public void AssertImageRepositorySaveWasCalledOnce()
            {
                _scannedImageRepository
                    .ReceivedWithAnyArgs(1)
                    .InsertScanAsync(null, CancellationToken.None);
            }
        }
    }
}