using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Rocket.Api.Contracts;
using Rocket.Api.Contracts.Scans;
using Rocket.Domain.Enum;
using Rocket.Domain.Exceptions;
using Rocket.Tests.Unit.Extensions;
using Rocket.Web.Host.Api;
using Rocket.Web.Host.Authentication;
using Xunit;

namespace Rocket.Tests.Unit
{
    public class ApiRequestManagerTests
    {
        private readonly TestContext _context = new();

        [Fact]
        public async Task Test_Valid_Response()
        {
            _context.ArrangeExpectedParseResponseTypeAsJsonBody();
            await _context.ActMakeApiRequestGetAsync();
            _context.AssertResultIsCompleteMyScanItemDetail();
        }

        [Fact]
        public async Task Test_Unparseable_Response()
        {
            _context.ArrangeUnparsableJsonResponseBody();
            await _context.ActMakeApiRequestGetAsync();
            _context.AssertResultIsNullAndUnparseableWasLogged();
        }

        [Fact]
        public async Task Test_Empty_Response_Body()
        {
            _context.ArrangeEmptyResponseBody();
            await _context.ActMakeApiRequestGetAsync();
            _context.AssertResultIsNullAndEmptyWasLogged();
        }

        [Fact]
        public async Task Test_Unauthorized_Response()
        {
            _context.ArrangeUnauthorizedResponseCode();
            await _context.ActMakeApiRequestWithExceptionAsync();
            _context.AssertErrorCodes(ApiStatusCodeEnum.ServerError, HttpStatusCode.Unauthorized );
        }

        [Fact]
        public async Task Test_Api_Error_Response()
        {
            _context.ArrangeApiErrorResponseCode();
            await _context.ActMakeApiRequestWithExceptionAsync();
            _context.AssertErrorCodes(ApiStatusCodeEnum.FileDataCorrupted, HttpStatusCode.InternalServerError );
        }

        private class TestContext
        {
            private readonly IAuthenticatedApiClient _authenticatedApiClient;
            private readonly ILogger<ApiRequestManager> _logger;
            private readonly IFixture _fixture;
            private readonly ApiRequestManager _sut;
            private string _mockHttpResponseMessageContent;
            private ApiResponse _result;
            private RocketException _exception;

            public TestContext()
            {
                _fixture =
                    FixtureEx
                        .CreateNSubstituteFixture();

                _authenticatedApiClient = _fixture.Freeze<IAuthenticatedApiClient>();
                _logger = _fixture.Freeze<ILogger<ApiRequestManager>>();

                _sut =
                    new ApiRequestManager(
                        _logger,
                        _authenticatedApiClient
                    );
            }

            public void ArrangeUnparsableJsonResponseBody()
            {
                _mockHttpResponseMessageContent = "//sh[{398y4absdbdasb##$";
                SetupApiResponseWithHttpCode(HttpStatusCode.OK);
            }

            public void ArrangeEmptyResponseBody()
            {
                _mockHttpResponseMessageContent = string.Empty;
                SetupApiResponseWithHttpCode(HttpStatusCode.OK);
            }

            public void ArrangeUnauthorizedResponseCode()
            {
                _mockHttpResponseMessageContent = string.Empty;
                SetupApiResponseWithHttpCode(HttpStatusCode.Unauthorized);
            }

            public void ArrangeApiErrorResponseCode()
            {
                _mockHttpResponseMessageContent = """
                                                  { 
                                                  "error_code": "2005", 
                                                  "error_message": "file data corrupted"
                                                  }
                                                  """;
                SetupApiResponseWithHttpCode(HttpStatusCode.OK);
            }

            public void ArrangeExpectedParseResponseTypeAsJsonBody()
            {
                _mockHttpResponseMessageContent = """
                                                  { 
                                                  "id": "abcdef",
                                                  "error_code": "0", 
                                                  "error_message": "no error",
                                                  "capture_date": "1909-01-01T00:00:00Z",
                                                  "blob_id": "923874ab3",
                                                  "content_type": "image/jpeg",
                                                  "file_extension": ".jpg",
                                                  "sha_256": "238736df872a",
                                                  "image_base64": "yyy==m"
                                                  }
                                                  """;
                SetupApiResponseWithHttpCode(HttpStatusCode.OK);
            }

            private void SetupApiResponseWithHttpCode(HttpStatusCode httpStatusCode)
            {
                var httpResponseMessage =
                    _fixture
                        .Build<HttpResponseMessage>()
                        .With(o => o.StatusCode, httpStatusCode)
                        .With(o => o.Content,
                            new StringContent(_mockHttpResponseMessageContent, Encoding.UTF8, "application/json"))
                        .Create();

                _authenticatedApiClient
                    .GetAsync(null, CancellationToken.None)
                    .ReturnsForAnyArgs(httpResponseMessage);
            }

            public async Task ActMakeApiRequestGetAsync()
            {
                _result =
                    await
                        _sut
                            .GetMyScanAsync(
                                null,
                                CancellationToken.None
                            );
            }

            public async Task ActMakeApiRequestWithExceptionAsync() =>
                _exception =
                    await
                        Assert
                            .ThrowsAsync<RocketException>(ActMakeApiRequestGetAsync);

            public void AssertResultIsCompleteMyScanItemDetail()
            {
                Assert.NotNull(_result);
                var myScanItemDetail = Assert.IsType<ScanSpecifics>(_result);
                Assert.Equal(0, myScanItemDetail.ErrorCode);
                Assert.Equal("abcdef", myScanItemDetail.Id);
                Assert.Equal("no error", myScanItemDetail.ErrorMessage);
                Assert.Equal(new DateTime(1909, 01, 01), myScanItemDetail.CaptureDate);
                Assert.Equal("923874ab3", myScanItemDetail.BlobId);
                Assert.Equal("image/jpeg", myScanItemDetail.ContentType);
                Assert.Equal(".jpg", myScanItemDetail.FileExtension);
                Assert.Equal("yyy==m", myScanItemDetail.ImageBase64);
            }

            public void AssertErrorCodes(
                ApiStatusCodeEnum expectedApiResponseCode,
                HttpStatusCode expectedHttpStatusCode)
            {
                Assert.Null(_result);
                Assert.NotNull(_exception);
                Assert.Equal(
                    (int)expectedApiResponseCode,
                    _exception.ApiStatusCode
                );
                Assert.Equal(
                    (int)expectedHttpStatusCode,
                    _exception.HttpStatusCode
                );
            }

            public void AssertResultIsNullAndUnparseableWasLogged()
            {
                Assert.Null(_result);
                _logger
                    .Received()
                    .Log(LogLevel.Information, 0, @"Received My Scan request");
                _logger
                    .Received()
                    .Log(LogLevel.Error, 0, @"Unexpected data received from endpoint: //sh[{398y4absdbdasb##$");
            }

            public void AssertResultIsNullAndEmptyWasLogged()
            {
                Assert.Null(_result);
                _logger
                    .Received()
                    .Log(LogLevel.Information, 0, @"Received My Scan request");
                _logger
                    .Received()
                    .Log(LogLevel.Error, 0, @"Unexpected data received from endpoint: ");
            }
        }
    }
}