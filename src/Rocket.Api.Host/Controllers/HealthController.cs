using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Rocket.Localization;

namespace Rocket.Api.Host.Controllers
{
    [ApiController]
    [Route("/api/health")]
    public class HealthCheckController(
        IStringLocalizer<LocalizationResource> localizer,
        ILogger<HealthCheckController> logger
    ) : ControllerBase
    {
        [HttpGet]
        [EndpointSummary("Health check")]
        [EndpointGroupName("Status")]
        [EndpointDescription("Returns the current health of the server. Returns 'Status OK' via a HTTP 200 response, if healthy.")]
        [ProducesResponseType(
            typeof(string),
            StatusCodes.Status200OK
        )]
        public IActionResult GetHealthCheck()
        {
            logger
                .LogInformation("Received health check request");

            return
                new OkObjectResult(localizer["HealthCheckOk"]);
        }
    }
}