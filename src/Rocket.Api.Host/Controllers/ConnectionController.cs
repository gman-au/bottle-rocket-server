using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
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
        [EndpointSummary("Connection health check (authenticated)")]
        [EndpointGroupName("Status")]
        [EndpointDescription("This endpoint will provide a status of the authenticated connection. Unauthorised or inactive accounts will return an error response.")]
        [Authorize]
        public async Task<IActionResult> GetConnectionTest()
        {
            logger
                .LogInformation("Received connection test request");

            await
                Task
                    .Delay(2000);
            
            return
                new ConnectionTestResponse()
                    .AsApiSuccess();
        }
    }
}