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
using Rocket.Domain.Enum;
using Rocket.Domain.Exceptions;
using Rocket.Google.Contracts;
using Rocket.Google.Domain;
using Rocket.Google.Infrastructure;
using Rocket.Interfaces;

namespace Rocket.Api.Host.Controllers.Vendors
{
    [ApiController]
    [Authorize]
    public class GoogleController(
        ILogger<GoogleController> logger,
        IGoogleTokenAcquirer tokenAcquirer,
        IConnectorRepository connectorRepository,
        IConnectorModelMapperRegistry connectorModelMapperRegistry,
        IUserManager userManager
    ) : RocketControllerBase(userManager)
    {
        [HttpPost, Route("/api/google/connectors/initiate")]
        [EndpointSummary("Initiate a Google authentication request")]
        [EndpointGroupName("Manage connectors")]
        [EndpointDescription(
            """
            Initiates a Google authentication request for the given user and client ID.\n
            The API will return an authorization URL that can be supplied back to the user
            to complete the authentication process.
            """
        )]
        [ProducesResponseType(
            typeof(GoogleAuthInitiateResponse),
            StatusCodes.Status200OK
        )]
        [ProducesResponseType(
            typeof(ApiResponse),
            StatusCodes.Status500InternalServerError
        )]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> InitiateAuthRequestAsync(
            [FromBody] GoogleAuthInitiateRequest request,
            CancellationToken cancellationToken
        )
        {
            var user =
                await
                    ThrowIfNotActiveUserAsync(cancellationToken);

            logger
                .LogInformation(
                    "Received (Google) auth request for username: {username}",
                    user.Username
                );

            var userId =
                user
                    .Id;

            var connector =
                await
                    connectorRepository
                        .GetConnectorByNameAsync<GoogleConnector>(
                            userId,
                            GoogleDomainConstants.ConnectorName,
                            cancellationToken
                        );

            if (connector == null)
            {
                var connectorSpecifics =
                    new GoogleConnectorSpecifics
                    {
                        UserId = userId,
                        CredentialsFileBase64 = request.CredentialsFileBase64
                    };

                var mapper =
                    connectorModelMapperRegistry
                        .GetMapperForView(typeof(GoogleConnectorSpecifics));

                connector =
                    mapper
                        .For(connectorSpecifics) as GoogleConnector;

                connector =
                    await
                        connectorRepository
                            .InsertConnectorAsync(
                                connector,
                                cancellationToken
                            ) as GoogleConnector;

                if (connector == null)
                    throw new RocketException(
                        "The Google connector could not be created.",
                        ApiStatusCodeEnum.ServerError
                    );
            }

            var authUrl =
                await
                    tokenAcquirer
                        .GetAuthorizationUrlAsync(
                            connector.Credential,
                            cancellationToken
                        );

            return
                new GoogleAuthInitiateResponse
                    {
                        Id = connector.Id,
                        AuthorizationUrl = authUrl
                    }
                    .AsApiSuccess();
        }

        [HttpPatch, Route("/api/google/connectors/finalize")]
        [EndpointSummary("Finalize a Google connector")]
        [EndpointGroupName("Manage connectors")]
        [EndpointDescription(
            """
            Finalizes a Google connector for the given user.\n
            When this endpoint is called, it is assumed that the user has already navigated to the Google authorization URL,
            authenticated, approved permissions, and copied the access code into the form / API request.
            The access code will be sent back to Google 
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
            [FromBody] GoogleAuthFinalizeRequest request,
            CancellationToken cancellationToken
        )
        {
            var user =
                await
                    ThrowIfNotActiveUserAsync(cancellationToken);

            logger
                .LogInformation(
                    "Received (Google) connector finalize request for username: {username}",
                    user.Username
                );

            var userId =
                user
                    .Id;

            var id = request.Id;

            var connector =
                await
                    connectorRepository
                        .GetConnectorByIdAsync<GoogleConnector>(
                            userId,
                            id,
                            cancellationToken
                        );

            if (connector == null)
                throw new RocketException(
                    "Connector entry not found",
                    ApiStatusCodeEnum.UnknownOrInaccessibleRecord
                );

            var accessCode = request.AccessCode;

            var (accessToken, refreshToken) =
                await
                    tokenAcquirer
                        .GetRefreshTokenFromAccessCodeAsync(
                            connector.Credential,
                            accessCode,
                            cancellationToken
                        );

            if (!string.IsNullOrEmpty(refreshToken))
            {
                await
                    connectorRepository
                        .UpdateConnectorFieldAsync<GoogleConnector, string>(
                            id,
                            userId,
                            o =>
                                o.AccessToken,
                            accessToken,
                            cancellationToken
                        );
                
                await
                    connectorRepository
                        .UpdateConnectorFieldAsync<GoogleConnector, string>(
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
                    .UpdateConnectorFieldAsync<GoogleConnector, DateTime?>(
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