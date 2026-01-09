using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Rocket.Api.Contracts;
using Rocket.Api.Host.Extensions;
using Rocket.Domain.Utils;
using Rocket.Interfaces;

namespace Rocket.Api.Host.Controllers
{
    [ApiController]
    [Route("/api/connection")]
    public class ConnectionController(
        ILogger<ConnectionController> logger,
        IUserManager userManager
    ) : RocketControllerBase(userManager)
    {
        [HttpPost]
        [Authorize]
        [EndpointSummary("Connection health check (authenticated)")]
        [EndpointGroupName("Status")]
        [EndpointDescription(
            """
            This endpoint will provide a status of the authenticated connection.\n
            Unauthorised or inactive accounts will return an error response.
            """
        )]
        [ProducesResponseType(
            typeof(ConnectionTestResponse),
            StatusCodes.Status200OK
        )]
        [ProducesResponseType(
            typeof(ApiResponse),
            StatusCodes.Status500InternalServerError
        )]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetConnectionTest(CancellationToken cancellationToken)
        {
            logger
                .LogInformation("Received connection test request");

            await
                Task
                    .Delay(
                        100,
                        cancellationToken
                    );

            var user =
                await
                    GetLoggedInUserAsync(cancellationToken);

            return
                new ConnectionTestResponse
                    {
                        UserName = user.Username,
                        Role = user.IsAdmin ? DomainConstants.AdminRole : DomainConstants.UserRole
                    }
                    .AsApiSuccess();
        }
    }
}