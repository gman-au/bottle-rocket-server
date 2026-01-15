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
using Rocket.Domain.Utils;
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
            Retrieves a subset of connectors belonging to the authenticated user.\n
            Provide a zero-based start index and record count to retrieve paged results and minimise server load. 
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
            var user =
                await
                    ThrowIfNotActiveUserAsync(cancellationToken);

            var userId =
                user
                    .Id;

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
                            .Select(o =>
                                new ConnectorSummary
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
                                    LastUpdatedAt = o.LastUpdatedAt?.ToLocalTime(),
                                    Status = (int)o.DetermineStatus()
                                }
                            ),
                    TotalRecords = (int)totalRecordCount
                };

            return
                response
                    .AsApiSuccess();
        }

        [HttpDelete("{id}")]
        [EndpointSummary("Delete a connector")]
        [EndpointGroupName("Manage connectors")]
        [EndpointDescription(
            """
            Deletes a user's connector by its unique ID.\n
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