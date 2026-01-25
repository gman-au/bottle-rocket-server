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
using Rocket.Domain.Enum;
using Rocket.Domain.Exceptions;
using Rocket.Interfaces;

namespace Rocket.Api.Host.Controllers
{
    [ApiController]
    [Route("/api/connectors")]
    [Authorize]
    public class ConnectorController(
        ILogger<ConnectorController> logger,
        IUserManager userManager,
        IConnectorModelMapperRegistry connectorModelMapperRegistry,
        IConnectorRepository connectorRepository
    ) : RocketControllerBase(userManager)
    {
        [HttpPost("fetch")]
        [EndpointSummary("Fetch the user connectors")]
        [EndpointGroupName("Manage connectors")]
        [EndpointDescription(
            """
            Retrieves a subset of connectors belonging to the authenticated user.\n
            Provide a zero-based start index and record count to retrieve paged results and minimise server load.\n
            If required, a connector code can be supplied as an additional filter.
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
                string.IsNullOrEmpty(request.Code)
                    ? await
                        connectorRepository
                            .FetchConnectorsAsync(
                                userId,
                                request.StartIndex.GetValueOrDefault(),
                                request.RecordCount.GetValueOrDefault(),
                                cancellationToken
                            )
                    : await
                        connectorRepository
                            .FetchConnectorsByCodeAndUserAsync(
                                userId,
                                request.StartIndex,
                                request.RecordCount,
                                request.Code,
                                cancellationToken
                            );

            var response =
                new FetchConnectorsResponse
                {
                    Connectors =
                        records
                            .Select(
                                o =>
                                {
                                    var mapper =
                                        connectorModelMapperRegistry
                                            .GetMapperForDomain(o.GetType());

                                    return
                                        mapper
                                            .From(o);
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
            Note: deleting a connector will remove all references to it from all associated workflows.
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

        [HttpPost, Route("/api/connectors/create")]
        [EndpointSummary("Add a new connector")]
        [EndpointGroupName("Manage connectors")]
        [EndpointDescription(
            """
            Creates a new connector for the given user. Will return an error if the same named
            connector already exists for the given user.
            """
        )]
        [ProducesResponseType(
            typeof(CreateConnectorResponse<ConnectorSummary>),
            StatusCodes.Status200OK
        )]
        [ProducesResponseType(
            typeof(ApiResponse),
            StatusCodes.Status500InternalServerError
        )]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CreateConnectorAsync(
            [FromBody] CreateConnectorRequest<ConnectorSummary> request,
            CancellationToken cancellationToken
        )
        {
            var user =
                await
                    ThrowIfNotActiveUserAsync(cancellationToken);

            logger
                .LogInformation(
                    "Received connector creation request for username: {username}",
                    user.Username
                );

            var userId =
                user
                    .Id;

            if (request.Connector == null)
                throw new RocketException(
                    "Could not determine connector from request",
                    ApiStatusCodeEnum.ValidationError
                );

            var connector =
                request
                    .Connector;

            connector.UserId = userId;

            var mapper =
                connectorModelMapperRegistry
                    .GetMapperForView(connector.GetType());

            var newConnector =
                mapper
                    .For(connector);

            if (await
                connectorRepository
                    .ConnectorExistsForUserAsync(
                        userId,
                        newConnector.ConnectorName,
                        cancellationToken
                    )
               )
                throw new RocketException(
                    $"Connector with name {newConnector.ConnectorName} already exists",
                    ApiStatusCodeEnum.RecordAlreadyExists
                );

            await
                mapper
                    .PreUpdateAsync(request.Connector);

            var result =
                await
                    connectorRepository
                        .InsertConnectorAsync(
                            newConnector,
                            cancellationToken
                        );

            if (result == null)
                throw new RocketException(
                    "Failed to create connector",
                    ApiStatusCodeEnum.ServerError
                );

            var response =
                await
                    mapper
                        .PostUpdateAsync(result);

            return
                response
                    .AsApiSuccess();
        }
    }
}