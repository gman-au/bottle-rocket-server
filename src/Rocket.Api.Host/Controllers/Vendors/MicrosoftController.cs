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
using Rocket.Interfaces;
using Rocket.Microsofts.Contracts;
using Rocket.Microsofts.Domain;
using Rocket.Microsofts.Infrastructure;

namespace Rocket.Api.Host.Controllers.Vendors
{
    [ApiController]
    [Authorize]
    public class MicrosoftController(
        ILogger<MicrosoftController> logger,
        IMicrosoftTokenAcquirer tokenAcquirer,
        IOneNoteSectionSearcher oneNoteSectionSearcher,
        IConnectorRepository connectorRepository,
        IUserManager userManager
    ) : RocketControllerBase(userManager)
    {
        [HttpPost, Route("/api/microsoft/connectors/initiate")]
        [EndpointSummary("Initiate a Microsoft authentication request")]
        [EndpointGroupName("Manage connectors")]
        [EndpointDescription(
            """
            // TODO
            """
        )]
        [ProducesResponseType(
            typeof(MicrosoftAuthInitiateResponse),
            StatusCodes.Status200OK
        )]
        [ProducesResponseType(
            typeof(ApiResponse),
            StatusCodes.Status500InternalServerError
        )]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> InitiateAuthRequestAsync(
            [FromBody] MicrosoftAuthInitiateRequest request,
            CancellationToken cancellationToken
        )
        {
            var user =
                await
                    ThrowIfNotActiveUserAsync(cancellationToken);

            logger
                .LogInformation(
                    "Received (Microsoft) auth request for username: {username}",
                    user.Username
                );

            var userId =
                user
                    .Id;

            var connector =
                await
                    connectorRepository
                        .GetConnectorByNameAsync<MicrosoftConnector>(
                            userId,
                            MicrosoftDomainConstants.ConnectorName,
                            cancellationToken
                        );

            if (connector == null)
            {
                connector =
                    new MicrosoftConnector
                    {
                        UserId = userId,
                        ClientId = request.ClientId,
                        TenantId = request.TenantId,
                        CreatedAt = DateTime.UtcNow,
                        LastUpdatedAt = DateTime.UtcNow
                    };

                connector =
                    await
                        connectorRepository
                            .InsertConnectorAsync(
                                connector,
                                cancellationToken
                            ) as MicrosoftConnector;
            }

            var response =
                await
                    tokenAcquirer
                        .AcquireAccountIdentifierAsync(
                            connector,
                            userId,
                            cancellationToken
                        );

            return
                new MicrosoftAuthInitiateResponse
                    {
                        Result = response
                    }
                    .AsApiSuccess();
        }

        [HttpPost, Route("/api/microsoft/workflows/getOneNoteSections")]
        [EndpointSummary("Get all OneNote sections accessible via a Microsoft connector")]
        [EndpointGroupName("Manage workflows")]
        [EndpointDescription(
            """
            Retrieves a set of (permitted) note sections for the given Microsoft connector.
            """
        )]
        [ProducesResponseType(
            typeof(GetOneNoteSectionsResponse),
            StatusCodes.Status200OK
        )]
        [ProducesResponseType(
            typeof(ApiResponse),
            StatusCodes.Status500InternalServerError
        )]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAllOneNoteSectionsAsync(
            [FromBody] GetOneNoteSectionsRequest request,
            CancellationToken cancellationToken
        )
        {
            var user =
                await
                    ThrowIfNotActiveUserAsync(cancellationToken);

            logger
                .LogInformation(
                    "Received (Microsoft) sections request for username: {username}",
                    user.Username
                );

            var userId =
                user
                    .Id;

            var connectorId = request.ConnectorId;

            var connector =
                await
                    connectorRepository
                        .GetConnectorByIdAsync<MicrosoftConnector>(
                            userId,
                            connectorId,
                            cancellationToken
                        );

            if (connector == null)
                throw new RocketException(
                    "Connector entry not found",
                    ApiStatusCodeEnum.UnknownOrInaccessibleRecord
                );

            // connect to notion and search for pages
            var sections =
                await
                    oneNoteSectionSearcher
                        .GetSectionsAsync(
                            connector,
                            cancellationToken
                        );

            return
                new GetOneNoteSectionsResponse
                    {
                        Sections = sections
                    }
                    .AsApiSuccess();
        }
    }
}