using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Rocket.Api.Contracts;
using Rocket.Api.Host.Extensions;
using Rocket.Interfaces;

namespace Rocket.Api.Host.Controllers
{
    [ApiController]
    [Route("/api/startup")]
    public class StartupController(
        IStartupInitialization startupInitialization,
        ILogger<StartupController> logger)
        : ControllerBase
    {
        [HttpGet("phase")]
        [EndpointSummary("Get the startup phase")]
        [EndpointGroupName("Status")]
        [EndpointDescription(
            """
            Gets the current startup phase of the system.\n
            After a new server installation, it can be one of three phases:\n
            0 = NoUserAccounts - unlikely, but possible if an operation to insert the admin account was aborted.\n
            1 = AdminPendingDeactivation - the first operating state of the server. A new admin account must be added to replace the temporary admin account.\n
            2 = AdminDeactivated - the phase after setup completion - a non-admin user account is managing the server and the admin account have been deactivated.
            """
        )]
        public async Task<IActionResult> GetPhaseAsync(CancellationToken cancellationToken)
        {
            logger
                .LogInformation("Received startup phase request");

            var phase = 
                await 
                    startupInitialization
                        .GetStartupPhaseAsync(cancellationToken);

            var response =
                new StartupPhaseResponse
                {
                    Phase = (int)phase
                };

            return 
                response
                    .AsApiSuccess();
        }
    }
}