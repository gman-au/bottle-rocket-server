using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Rocket.Api.Contracts;
using Rocket.Api.Host.Extensions;

namespace Rocket.Api.Host.Controllers
{
    [ApiController]
    [Route("/api/connection")]
    public class ConnectionController(ILogger<ConnectionController> logger) : ControllerBase
    {
        [HttpPost]
        public IActionResult GetConnectionTest()
        {
            logger
                .LogInformation("Received connection test request");

            return
                new ConnectionTestResponse()
                    .AsApiSuccess();
        }
    }
}