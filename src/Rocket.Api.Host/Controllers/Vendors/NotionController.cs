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
using Rocket.Notion.Contracts;
using Rocket.Notion.Domain;
using Rocket.Notion.Infrastructure;

namespace Rocket.Api.Host.Controllers.Vendors
{
    [ApiController]
    [Authorize]
    public class NotionController(
        ILogger<NotionController> logger,
        INotionNoteSearcher notionNoteSearcher,
        IConnectorRepository connectorRepository,
        IUserManager userManager
    ) : RocketControllerBase(userManager)
    {
        [HttpPost, Route("/api/notion/workflows/getParentNotes")]
        [EndpointSummary("Get all parent notes accessible via a Notion connector")]
        [EndpointGroupName("Manage connectors")]
        [EndpointDescription(
            """
            Retrieves a set of (permitted) parent notes for the given Notion connector.\n
            The notes that a Notion integration has access to are explicitly defined in the Notion integration.
            """
        )]
        [ProducesResponseType(
            typeof(GetAllNotionParentNotesResponse),
            StatusCodes.Status200OK
        )]
        [ProducesResponseType(
            typeof(ApiResponse),
            StatusCodes.Status500InternalServerError
        )]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAllNotionParentNotesAsync(
            [FromBody] GetAllNotionParentNotesRequest request,
            CancellationToken cancellationToken
        )
        {
            var user =
                await
                    ThrowIfNotActiveUserAsync(cancellationToken);

            logger
                .LogInformation(
                    "Received (Notion) parent notes request for username: {username}",
                    user.Username
                );

            var userId =
                user
                    .Id;

            var connectorId = request.ConnectorId;

            var connector =
                await
                    connectorRepository
                        .GetConnectorByIdAsync<NotionConnector>(
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
            var parentNotes =
                await
                    notionNoteSearcher
                        .GetParentNotesAsync(
                            connector.IntegrationSecret,
                            cancellationToken
                        );

            return
                new GetAllNotionParentNotesResponse
                    {
                        ParentNotes = parentNotes
                    }
                    .AsApiSuccess();
        }
    }
}