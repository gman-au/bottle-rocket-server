using System.Reflection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Rocket.Api.Contracts;
using Rocket.Api.Host.Extensions;

namespace Rocket.Api.Host.Controllers
{
    [ApiController]
    [Route("/api/version")]
    public class VersionController(ILogger<VersionController> logger) : ControllerBase
    {
        [HttpGet]
        [EndpointSummary("Version")]
        [EndpointGroupName("Status")]
        [EndpointDescription("Returns the current version server API.")]
        [ProducesResponseType(
            typeof(VersionResponse),
            StatusCodes.Status200OK
        )]
        public IActionResult GetHealthCheck()
        {
            logger
                .LogInformation("Received version request");

            var apiVersion =
                Assembly
                    .GetExecutingAssembly()
                    .GetName()
                    .Version
                    ?.ToString()
                ?? "dev";

            var response = new VersionResponse
            {
                ApiVersion = apiVersion,
                WebVersion = null
            };

            return
                response
                    .AsApiSuccess();
        }
    }
}