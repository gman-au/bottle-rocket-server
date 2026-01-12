using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Rocket.Api.Contracts;
using Rocket.Api.Contracts.Connectors;
using Rocket.Api.Host.Extensions;
using Rocket.Domain.Connectors;
using Rocket.Domain.Enum;
using Rocket.Domain.Exceptions;
using Rocket.Interfaces;

namespace Rocket.Api.Host.Controllers.Vendors
{
    [ApiController]
    [Route("/api/connectors/dropbox")]
    [Authorize]
    public class DropboxController(
        ILogger<DropboxController> logger,
        IUserManager userManager,
        IConnectorRepository connectorRepository
    ) : RocketControllerBase(userManager)
    {
        [HttpPost("create")]
        [EndpointSummary("Add a new Dropbox connector")]
        [EndpointGroupName("Manage connectors")]
        [EndpointDescription(
            """
            TODO
            """
        )]
        [ProducesResponseType(
            typeof(DropboxConnectorDetail),
            StatusCodes.Status200OK
        )]
        [ProducesResponseType(
            typeof(ApiResponse),
            StatusCodes.Status500InternalServerError
        )]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CreateConnectorAsync(
            [FromBody] DropboxConnectorDetail request,
            CancellationToken cancellationToken
        )
        {
            var user =
                await
                    ThrowIfNotActiveUserAsync(cancellationToken);

            logger
                .LogInformation(
                    "Received (Dropbox) connector creation request for username: {username}",
                    user.Username
                );

            var userId =
                user
                    .Id;

            if (string.IsNullOrEmpty(request.AccessToken))
                throw new RocketException(
                    "No API token was provided.",
                    ApiStatusCodeEnum.ValidationError
                );

            var newConnector =
                new DropboxConnector
                {
                    UserId = userId,
                    CreatedAt = DateTime.UtcNow,
                    LastUpdatedAt = DateTime.UtcNow,
                    AccessToken = request.AccessToken
                };

            var result =
                await
                    connectorRepository
                        .SaveConnectorAsync(
                            newConnector,
                            cancellationToken
                        );

            var response =
                new CreateConnectorResponse
                {
                    Id = result.Id,
                    CreatedAt = result.CreatedAt.ToLocalTime()
                };

            return
                response
                    .AsApiSuccess();
        }
        
        [HttpPatch("update")]
        [EndpointSummary("Update a Dropbox connector")]
        [EndpointGroupName("Manage connectors")]
        [EndpointDescription(
            """
            TODO
            """
        )]
        [ProducesResponseType(
            typeof(DropboxConnectorDetail),
            StatusCodes.Status200OK
        )]
        [ProducesResponseType(
            typeof(ApiResponse),
            StatusCodes.Status500InternalServerError
        )]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> UpdateConnectorAsync(
            [FromBody] DropboxConnectorDetail request,
            CancellationToken cancellationToken
        )
        {
            var user =
                await
                    ThrowIfNotActiveUserAsync(cancellationToken);

            logger
                .LogInformation(
                    "Received (Dropbox) connector update request for username: {username}",
                    user.Username
                );

            var userId =
                user
                    .Id;

            if (string.IsNullOrEmpty(request.AccessToken))
                throw new RocketException(
                    "No API token was provided.",
                    ApiStatusCodeEnum.ValidationError
                );
            
            var connectorId = request.Id;

            await
                connectorRepository
                    .UpdateConnectorFieldAsync<DropboxConnector, string>(
                        connectorId,
                        userId,
                        o =>
                            o.AccessToken,
                        request.AccessToken,
                        cancellationToken
                    );

            var lastUpdatedAt = DateTime.UtcNow;
            
            await
                connectorRepository
                    .UpdateConnectorFieldAsync<DropboxConnector, DateTime?>(
                        connectorId,
                        userId,
                        o =>
                            o.LastUpdatedAt,
                        lastUpdatedAt,
                        cancellationToken
                    );

            var response =
                new UpdateConnectorResponse
                {
                    Id = connectorId,
                    LastUpdatedAt = lastUpdatedAt
                };

            return
                response
                    .AsApiSuccess();
        }
    }
}