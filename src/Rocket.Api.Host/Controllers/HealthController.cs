using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Rocket.Api.Host.Controllers
{
    [ApiController]
    [Route("/api/health")]
    public class HealthCheckController(ILogger<HealthCheckController> logger) : ControllerBase
    {
        [HttpGet]
        public IActionResult GetHealthCheck()
        {
            logger
                .LogInformation("Received health check request");

            return new OkObjectResult("Status OK");
        }
    }
}