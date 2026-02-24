using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Rocket.Api.Contracts;
using Rocket.Api.Contracts.GlobalSettings;
using Rocket.Api.Host.Extensions;
using Rocket.Domain.Enum;
using Rocket.Domain.Exceptions;
using Rocket.Interfaces;

namespace Rocket.Api.Host.Controllers
{
    [ApiController]
    [Route("/api/globalSettings")]
    [Authorize]
    public class GlobalSettingsController(
        ILogger<GlobalSettingsController> logger,
        IGlobalSettingsRepository globalSettingsRepository,
        IGlobalSettingsChangedSignal globalSettingsChangedSignal,
        IUserManager userManager
    ) : RocketControllerBase(userManager)
    {
        [HttpGet("get")]
        [EndpointSummary("Get global settings")]
        [EndpointGroupName("Manage server")]
        [EndpointDescription("Returns the current global system settings.")]
        [ProducesResponseType(
            typeof(GlobalSettingsSpecifics),
            StatusCodes.Status200OK
        )]
        [ProducesResponseType(
            typeof(ApiResponse),
            StatusCodes.Status500InternalServerError
        )]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetSettingsAsync(CancellationToken cancellationToken)
        {
            var user =
                await
                    ThrowIfNotAdminAsync(cancellationToken);

            logger
                .LogInformation(
                    "Received global settings request from user {userId}",
                    user.Id
                );

            var record =
                await
                    globalSettingsRepository
                        .GetGlobalSettingsAsync(cancellationToken);

            if (record == null)
                throw new RocketException(
                    "No settings could be found. Please restart the server and try again.",
                    ApiStatusCodeEnum.ServerError
                );

            var response =
                new GlobalSettingsSpecifics
                {
                    SweepSuccessfulScansAfterDays = record.SweepSuccessfulScansAfterDays,
                    EnableSweeping = record.EnableSweeping
                };

            return
                response
                    .AsApiSuccess();
        }

        [HttpPost("update")]
        [EndpointSummary("Update global settings")]
        [EndpointGroupName("Manage server")]
        [EndpointDescription(
            """
            Updates the global system settings. Services that are driven by global system configuration will be restarted e.g. sweeping jobs.
            """
        )]
        [ProducesResponseType(
            typeof(ApiResponse),
            StatusCodes.Status200OK
        )]
        [ProducesResponseType(
            typeof(ApiResponse),
            StatusCodes.Status500InternalServerError
        )]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> UpdateSettingsAsync(
            [FromBody] GlobalSettingsSpecifics request,
            CancellationToken cancellationToken
        )
        {
            var user =
                await
                    ThrowIfNotAdminAsync(cancellationToken);

            logger
                .LogInformation(
                    "Received global settings update request for user: {id}",
                    user.Id
                );

            await
                globalSettingsRepository
                    .UpdateGlobalSettingsAsync(
                        request.SweepSuccessfulScansAfterDays,
                        request.EnableSweeping,
                        cancellationToken
                    );

            logger
                .LogInformation(
                    "Updated global settings"
                );

            // anything dependent on global settings should be notified
            globalSettingsChangedSignal
                .NotifyChanged();

            return
                new ApiResponse()
                    .AsApiSuccess();
        }
    }
}