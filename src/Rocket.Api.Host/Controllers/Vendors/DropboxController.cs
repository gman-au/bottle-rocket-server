using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Rocket.Api.Contracts;
using Rocket.Api.Host.Extensions;
using Rocket.Domain.Connectors;
using Rocket.Domain.Enum;
using Rocket.Domain.Exceptions;
using Rocket.Domain.Utils;
using Rocket.Integrations.Dropbox;
using Rocket.Integrations.Dropbox.Contracts;
using Rocket.Interfaces;

namespace Rocket.Api.Host.Controllers.Vendors
{
    [ApiController]
    [Route("/api/connectors/dropbox")]
    [Authorize]
    public class DropboxController(
        ILogger<DropboxController> logger,
        IDropboxClientManager dropboxClientManager,
        IUserManager userManager,
        IConnectorRepository connectorRepository
    ) : RocketControllerBase(userManager)
    {
        [HttpPost("create")]
        [EndpointSummary("Add a new Dropbox connector")]
        [EndpointGroupName("Manage connectors")]
        [EndpointDescription(
            """
            Creates a new Dropbox connector for the given user. Will return an error if the same named
            connector already exists for the given user.
            """
        )]
        [ProducesResponseType(
            typeof(CreateDropboxConnectorResponse),
            StatusCodes.Status200OK
        )]
        [ProducesResponseType(
            typeof(ApiResponse),
            StatusCodes.Status500InternalServerError
        )]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CreateConnectorAsync(
            [FromBody] CreateDropboxConnectorRequest request,
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

            if (await
                connectorRepository
                    .ConnectorExistsForUserAsync(
                        userId,
                        DomainConstants.VendorDropbox,
                        cancellationToken
                    )
               )
                throw new RocketException(
                    "Dropbox connector already exists",
                    ApiStatusCodeEnum.ConnectorAlreadyExists
                );

            if (string.IsNullOrEmpty(request.AppKey))
                throw new RocketException(
                    "No app key was provided.",
                    ApiStatusCodeEnum.ValidationError
                );

            if (string.IsNullOrEmpty(request.AppSecret))
                throw new RocketException(
                    "No app secret was provided.",
                    ApiStatusCodeEnum.ValidationError
                );

            var newConnector =
                new DropboxConnector
                {
                    UserId = userId,
                    CreatedAt = DateTime.UtcNow,
                    LastUpdatedAt = DateTime.UtcNow,
                    AppKey = request.AppKey,
                    AppSecret = request.AppSecret
                };

            var result =
                await
                    connectorRepository
                        .SaveConnectorAsync(
                            newConnector,
                            cancellationToken
                        );

            if (result == null)
                throw new RocketException(
                    "Failed to create Dropbox connector",
                    ApiStatusCodeEnum.ServerError
                );

            var authorizeUri =
                dropboxClientManager
                    .GetAuthorizeUrl(request.AppKey);

            var response =
                new CreateDropboxConnectorResponse
                {
                    Id = result.Id,
                    AuthorizeUri = authorizeUri
                };

            return
                response
                    .AsApiSuccess();
        }

        [HttpPatch("finalize")]
        [EndpointSummary("Finalize a Dropbox connector")]
        [EndpointGroupName("Manage connectors")]
        [EndpointDescription(
            """
            Finalizes a Dropbox connector for the given user.
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
        public async Task<IActionResult> FinalizeConnectorAsync(
            [FromBody] FinalizeDropboxConnectorRequest request,
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

            var id = request.Id;

            var connector =
                await
                    connectorRepository
                        .FetchConnectorAsync(
                            userId,
                            id,
                            cancellationToken
                        ) as DropboxConnector;

            if (connector == null)
                throw new RocketException(
                    "Connector entry not found",
                    ApiStatusCodeEnum.UnknownOrInaccessibleRecord
                );

            var appKey = connector.AppKey;
            var appSecret = connector.AppSecret;
            var accessCode = request.AccessCode;

            var refreshToken =
                await
                    dropboxClientManager
                        .GetRefreshTokenAsync(
                            appKey,
                            appSecret,
                            accessCode
                        );

            if (!string.IsNullOrEmpty(refreshToken))
            {
                // swap code for token
                await
                    connectorRepository
                        .UpdateConnectorFieldAsync<DropboxConnector, string>(
                            id,
                            userId,
                            o =>
                                o.RefreshToken,
                            refreshToken,
                            cancellationToken
                        );
            }

            var lastUpdatedAt = DateTime.UtcNow;

            await
                connectorRepository
                    .UpdateConnectorFieldAsync<DropboxConnector, DateTime?>(
                        id,
                        userId,
                        o =>
                            o.LastUpdatedAt,
                        lastUpdatedAt,
                        cancellationToken
                    );

            return
                new ApiResponse()
                    .AsApiSuccess();
        }
    }
}