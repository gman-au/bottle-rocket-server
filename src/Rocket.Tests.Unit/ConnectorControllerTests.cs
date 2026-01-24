using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Rocket.Api.Host.Controllers;
using Rocket.Domain.Connectors;
using Rocket.Domain.Core.Enum;
using Rocket.Domain.Exceptions;
using Rocket.Interfaces;
using Rocket.Tests.Unit.Utility;
using Xunit;

namespace Rocket.Tests.Unit
{
    public class ConnectorControllerTests
    {
        private readonly TestContext _context = new();

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
                        Fixture.Freeze<IConnectorModelMapperRegistry>(),
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
                        .With(o => o.ConnectorType, (int)ConnectorTypeEnum.FileForwarding)
                        .Create();

                _connectorRepository
                    .GetConnectorByIdAsync<DropboxConnector>(
                        null,
                        null,
                        CancellationToken.None
                    )
                    .ReturnsForAnyArgs(Task.FromResult(connector));
            }

            public void ArrangeUnknownConnectorReturned()
            {
                var connector =
                    Fixture
                        .Build<UnknownConnector>()
                        .Create();

                _connectorRepository
                    .GetConnectorByIdAsync<UnknownConnector>(
                        null,
                        null,
                        CancellationToken.None
                    )
                    .ReturnsForAnyArgs(Task.FromResult(connector));
            }

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

            private record UnknownConnector : BaseConnector
            {
                public override int ConnectorType { get; set; } = (int)ConnectorTypeEnum.FileForwarding;
                public override string ConnectorName { get; set; } = "UNKNOWN";
                
                public override string ConnectorCode { get; set; } = "UNKNOWN";
                public override ConnectorStatusEnum DetermineStatus() => ConnectorStatusEnum.Pending;
            }
        }
    }
}