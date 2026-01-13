using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Rocket.Api.Contracts.Connectors;
using Rocket.Api.Host.Controllers;
using Rocket.Domain.Connectors;
using Rocket.Domain.Enum;
using Rocket.Domain.Exceptions;
using Rocket.Interfaces;
using Rocket.Tests.Unit.Utility;
using Xunit;

namespace Rocket.Tests.Unit
{
    public class ConnectorControllerTests
    {
        private readonly TestContext _context = new();

        [Fact]
        public async Task Test_Connector_Get_As_Inactive_User()
        {
            _context.ArrangeLoggedInInactiveAdminUser();
            _context.ArrangeDropboxConnectorReturned();
            await _context.ActGetConnectorWithExceptionAsync();
            _context.AssertExceptionCode(ApiStatusCodeEnum.InactiveUser);
        }

        [Fact]
        public async Task Test_Unknown_Connector_Returned()
        {
            _context.ArrangeLoggedInNonAdminUser();
            _context.ArrangeUnknownConnectorReturned();
            await _context.ActGetConnectorWithExceptionAsync();
            _context.AssertExceptionCode(ApiStatusCodeEnum.ServerError);
        }

        [Fact]
        public async Task Test_Dropbox_Connector_Returned()
        {
            _context.ArrangeLoggedInNonAdminUser();
            _context.ArrangeDropboxConnectorReturned();
            await _context.ActGetConnectorAsync();
            _context.AssertOkResult();
            _context.AssertDropboxConnectorReturned();
        }

        private class TestContext : BaseControllerTestContext<ConnectorController>
        {
            private readonly IConnectorRepository _connectorRepository;
            private RocketException _exception;
            private IActionResult _result;

            public TestContext()
            {
                _connectorRepository = Fixture.Freeze<IConnectorRepository>();

                Sut =
                    new ConnectorController(
                        Fixture.Freeze<ILogger<ConnectorController>>(),
                        UserManager,
                        _connectorRepository
                    );

                SetupGetUserReturns();
                SetupControllerContexts();
            }

            public void ArrangeDropboxConnectorReturned()
            {
                var connector =
                    Fixture
                        .Build<DropboxConnector>()
                        .With(o => o.AccessToken, "1234567890abcdef")
                        .With(o => o.ConnectorType, (int)ConnectorTypeEnum.FileForwarding)
                        .Create();

                _connectorRepository
                    .FetchConnectorAsync(
                        null,
                        null,
                        CancellationToken.None
                    )
                    .ReturnsForAnyArgs(Task.FromResult<BaseConnector>(connector));
            }

            public void ArrangeUnknownConnectorReturned()
            {
                var connector =
                    Fixture
                        .Build<UnknownConnectorType>()
                        .Create();

                _connectorRepository
                    .FetchConnectorAsync(
                        null,
                        null,
                        CancellationToken.None
                    )
                    .ReturnsForAnyArgs(Task.FromResult<BaseConnector>(connector));
            }

            public async Task ActGetConnectorAsync()
            {
                _result =
                    await
                        Sut
                            .FetchConnectorAsync(
                                "123456",
                                CancellationToken.None
                            );
            }

            public async Task ActGetConnectorWithExceptionAsync() =>
                _exception =
                    await
                        Assert
                            .ThrowsAsync<RocketException>(ActGetConnectorAsync);

            public void AssertOkResult()
            {
                Assert.NotNull(_result);
                var okResult = Assert.IsType<OkObjectResult>(_result);
                Assert.Equal(
                    (int)HttpStatusCode.OK,
                    okResult.StatusCode
                );
            }

            public void AssertExceptionCode(ApiStatusCodeEnum expected)
            {
                Assert.Null(_result);
                Assert.NotNull(_exception);
                Assert.Equal(
                    (int)expected,
                    _exception.ApiStatusCode
                );
            }

            public void AssertDropboxConnectorReturned()
            {
                var okResult = Assert.IsType<OkObjectResult>(_result);
                var connector = Assert.IsType<DropboxConnectorDetail>(okResult.Value);
                Assert.Equal("****cdef", connector.AccessToken);
                Assert.Equal("File Forwarding", connector.ConnectorType);
            }

            private class UnknownConnectorType : BaseConnector
            {
                public override int ConnectorType { get; set; } = (int)ConnectorTypeEnum.FileForwarding;
                public override string ConnectorName { get; set; } = "UNKNOWN";
            }
        }
    }
}