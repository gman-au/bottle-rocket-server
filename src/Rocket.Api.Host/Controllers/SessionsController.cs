using System;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Rocket.Api.Contracts;
using Rocket.Api.Contracts.Sessions;
using Rocket.Api.Host.Extensions;
using Rocket.Domain.Enum;
using Rocket.Domain.Exceptions;
using Rocket.Infrastructure.Auth;
using Rocket.Interfaces;

namespace Rocket.Api.Host.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SessionsController(
        ILogger<SessionsController> logger,
        IUserManager userManager,
        ITokenSessionRepository tokenSessionRepository
    ) : RocketControllerBase(userManager)
    {
        private string CurrentSessionId => User.FindFirstValue(AuthConstants.SessionId)!;

        [Authorize]
        [HttpPost("fetch")]
        [EndpointSummary("Fetch the users current login sessions")]
        [EndpointGroupName("Authentication")]
        [EndpointDescription(
            """
            Retrieves a subset of current sessions belonging to the authenticated user.\n
            Provide a zero-based start index and record count to retrieve paged results and minimise server load. 
            """
        )]
        [ProducesResponseType(
            typeof(MySessionsResponse),
            StatusCodes.Status200OK
        )]
        [ProducesResponseType(
            typeof(ApiResponse),
            StatusCodes.Status500InternalServerError
        )]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> FetchMySessionsAsync(
            [FromBody] MySessionsRequest request,
            CancellationToken cancellationToken
        )
        {
            var userId =
                User
                    .FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                throw new RocketException(
                    "User ID not found in claims",
                    ApiStatusCodeEnum.UnknownUser,
                    (int)HttpStatusCode.Unauthorized
                );

            logger
                .LogInformation(
                    "Received fetch sessions request for username: {userId}",
                    userId
                );

            var (records, totalRecordCount) =
                await
                    tokenSessionRepository
                        .FetchSessionsAsync(
                            userId,
                            request.StartIndex,
                            request.RecordCount,
                            cancellationToken
                        );

            var response =
                new MySessionsResponse
                {
                    Sessions = 
                        records
                            .Select(
                                o =>
                                    new TokenSessionSpecifics
                                    {
                                        Id = o.Id,
                                        DeviceName = o.DeviceName,
                                        IpAddress = o.IpAddress,
                                        CreatedAt = o.CreatedAt,
                                        LastLoginAt = o.LastLoginAt,
                                        TokenPrefix = o.TokenPrefix,
                                        IsCurrent = o.Id == CurrentSessionId
                                    }
                            ),
                    TotalRecords = (int)totalRecordCount
                };

            return
                response
                    .AsApiSuccess();
        }

        [HttpDelete("revoke/{id}")]
        [EndpointSummary("Revoke a session")]
        [EndpointGroupName("Authentication")]
        [EndpointDescription(
            """
            Revokes a current user session by its unique ID.\n
            The session is removed from the system and any devices using the associated session will be signed out immediately.
            """
        )]
        [ProducesResponseType(
            typeof(RevokeSessionResponse),
            StatusCodes.Status200OK
        )]
        [ProducesResponseType(
            typeof(ApiResponse),
            StatusCodes.Status500InternalServerError
        )]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> RevokeSessionAsync(
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
                    "Received session revocation request for username: {userId}, id: {id}",
                    userId,
                    id
                );

            await
                tokenSessionRepository
                    .RevokeAsync(
                        userId,
                        id,
                        DateTime.UtcNow,
                        cancellationToken
                    );

            var response =
                new RevokeSessionResponse();

            return
                response
                    .AsApiSuccess();
        }
    }
}