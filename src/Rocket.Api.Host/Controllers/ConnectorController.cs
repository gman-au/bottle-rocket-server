using System.Collections.Generic;
using System.Linq;
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
using Rocket.Domain.Utils;
using Rocket.Infrastructure.Extensions;
using Rocket.Interfaces;

namespace Rocket.Api.Host.Controllers
{
    [ApiController]
    [Route("/api/connectors")]
    [Authorize]
    public class ConnectorController(
        ILogger<ConnectorController> logger,
        IUserManager userManager,
        IConnectorRepository connectorRepository
    ) : RocketControllerBase(userManager)
    {
        [HttpPost("fetch")]
        [EndpointSummary("Fetch the user connectors")]
        [EndpointGroupName("Manage connectors")]
        [EndpointDescription(
            """
            TODO
            """
        )]
        [ProducesResponseType(
            typeof(FetchConnectorsResponse),
            StatusCodes.Status200OK
        )]
        [ProducesResponseType(
            typeof(ApiResponse),
            StatusCodes.Status500InternalServerError
        )]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> FetchConnectorsAsync(
            [FromBody] FetchConnectorsRequest request,
            CancellationToken cancellationToken
        )
        {
            await
                ThrowIfNotActiveUserAsync(cancellationToken);

            var userId =
                GetLoggedInUserId();

            var (records, totalRecordCount) =
                await
                    connectorRepository
                        .FetchConnectorsAsync(
                            userId,
                            request.StartIndex,
                            request.RecordCount,
                            cancellationToken
                        );

            var response =
                new FetchConnectorsResponse
                {
                    Connectors =
                        records
                            .Select(
                                o =>
                                    new ConnectorItem
                                    {
                                        Id = o.Id,
                                        ConnectorType =
                                            DomainConstants
                                                .ConnectorTypes
                                                .GetValueOrDefault(
                                                    o.ConnectorType,
                                                    "Unknown"
                                                ),
                                        ConnectorName = o.ConnectorName,
                                        CreatedAt = o.CreatedAt.ToLocalTime(),
                                        LastUpdatedAt = o.LastUpdatedAt?.ToLocalTime()
                                    }
                            ),
                    TotalRecords = (int)totalRecordCount
                };

            return
                response
                    .AsApiSuccess();
        }

        [HttpGet("{id}")]
        [EndpointSummary("Fetch the connector details")]
        [EndpointGroupName("Manage connectors")]
        [EndpointDescription(
            """
            TODO
            """
        )]
        [ProducesResponseType(
            typeof(ConnectorDetail),
            StatusCodes.Status200OK
        )]
        [ProducesResponseType(
            typeof(ApiResponse),
            StatusCodes.Status500InternalServerError
        )]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> FetchConnectorAsync(
            string id,
            CancellationToken cancellationToken
        )
        {
            var user =
                await
                    ThrowIfNotActiveUserAsync(cancellationToken);

            var userId =
                user
                    .Id;

            logger
                .LogInformation(
                    "Received fetch connection request for username: {userId}, id: {id}",
                    userId,
                    id
                );


            var record =
                await
                    connectorRepository
                        .FetchConnectorAsync(
                            userId,
                            id,
                            cancellationToken
                        );

            return record switch
            {
                DropboxConnector dbc => new DropboxConnectorDetail
                {
                    Id = dbc.Id,
                    ConnectorType = DomainConstants.ConnectorTypes[dbc.ConnectorType],
                    ConnectorName = dbc.ConnectorName,
                    CreatedAt = dbc.CreatedAt.ToLocalTime(),
                    LastUpdatedAt = dbc.LastUpdatedAt?.ToLocalTime(),
                    AccessToken = dbc.AccessToken.Obfuscate()
                }.AsApiSuccess(),
                _ => throw new RocketException(
                    "Unknown connector type",
                    ApiStatusCodeEnum.ServerError
                )
            };
        }

        [HttpDelete("{id}")]
        [EndpointSummary("Delete a connector")]
        [EndpointGroupName("Manage connectors")]
        [EndpointDescription(
            """
            TODO
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
        public async Task<IActionResult> DeleteConnectorAsync(
            string id,
            CancellationToken cancellationToken
        )
        {
            var user =
                await
                    ThrowIfNotActiveUserAsync(cancellationToken);

            var userId =
                user
                    .Id;

            logger
                .LogInformation(
                    "Received connector deletion request for username: {userId}, id: {id}",
                    userId,
                    id
                );

            var result =
                await
                    connectorRepository
                        .DeleteConnectorAsync(
                            userId,
                            id,
                            cancellationToken
                        );

            var response =
                new DeleteConnectorResponse
                {
                    IsDeleted = result
                };

            return
                response
                    .AsApiSuccess();
        }
    }
}