using System;
using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Rocket.Api.Contracts;
using Rocket.Api.Host.Exceptions;
using Rocket.Domain.Core.Enum;
using Rocket.Domain.Exceptions;
using Rocket.Tests.Unit.Extensions;
using Xunit;

namespace Rocket.Tests.Unit
{
    public class RocketExceptionWrapperTests
    {
        private readonly TestContext _context = new();

        [Fact]
        private void Test_Non_Rocket_Exception_Is_Wrapped()
        {
            _context.ArrangeNonRocketException();
            _context.ActCatchException();
            _context.AssertHttp500CodeWithUnknownApiError();
        }

        [Fact]
        private void Test_Rocket_Explicit_Exception_Is_Wrapped()
        {
            _context.ArrangeExplicitRocketException();
            _context.ActCatchException();
            _context.AssertExplicitRocketExceptionIsWrapped();
        }

        [Fact]
        private void Test_Rocket_Implied_Exception_Is_Wrapped()
        {
            _context.ArrangeImpliedRocketStatusCodeException();
            _context.ActCatchException();
            _context.AssertImpliedRocketStatusCodeIsWrapped();
        }

        private class TestContext
        {
            private readonly RocketExceptionWrapper _sut;

            private Exception _exception;

            private ObjectResult _result;

            public TestContext()
            {
                var fixture =
                    FixtureEx
                        .CreateNSubstituteFixture();

                var loggerFactory =
                    fixture
                        .Freeze<ILoggerFactory>();

                _sut = new RocketExceptionWrapper(loggerFactory);
            }

            public void ArrangeNonRocketException()
            {
                _exception = new Exception();
            }

            public void ArrangeExplicitRocketException()
            {
                _exception =
                    new RocketException(
                        "I spill my drink",
                        ApiStatusCodeEnum.NoAttachmentsFound,
                        401
                    );
            }

            public void ArrangeImpliedRocketStatusCodeException()
            {
                _exception =
                    new RocketException(
                        apiStatusCode: 2000
                    );
            }

            public void ActCatchException()
            {
                _result =
                    _sut
                        .For(_exception);
            }

            public void AssertHttp500CodeWithUnknownApiError()
            {
                Assert.Equal(
                    500,
                    _result.StatusCode
                );
                var apiResponse = _result.Value as ApiResponse;
                Assert.NotNull(apiResponse);
                Assert.Equal(
                    1000,
                    apiResponse.ErrorCode
                );
            }

            public void AssertImpliedRocketStatusCodeIsWrapped()
            {
                Assert.Equal(
                    500,
                    _result.StatusCode
                );
                var apiResponse = _result.Value as ApiResponse;
                Assert.NotNull(apiResponse);
                Assert.Equal(
                    2000,
                    apiResponse.ErrorCode
                );
                Assert.Equal(
                    "There was an error.",
                    apiResponse.ErrorMessage
                );
            }

            public void AssertExplicitRocketExceptionIsWrapped()
            {
                Assert.Equal(
                    401,
                    _result.StatusCode
                );
                var apiResponse = _result.Value as ApiResponse;
                Assert.NotNull(apiResponse);
                Assert.Equal(
                    1001,
                    apiResponse.ErrorCode
                );
                Assert.Equal(
                    "I spill my drink",
                    apiResponse.ErrorMessage
                );
            }
        }
    }
}