using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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