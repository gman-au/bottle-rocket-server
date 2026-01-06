using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using Rocket.Api.Contracts;
using Rocket.Api.Host.Extensions;
using Rocket.Domain.Enum;
using Rocket.Domain.Exceptions;
using Rocket.Domain.Utils;
using Rocket.Interfaces;

namespace Rocket.Api.Host.Controllers
{
    [ApiController]
    [Route("/api/users")]
    [Authorize]
    public class UserController(
        ILogger<UserController> logger,
        IUserManager userManager,
        IStartupInitialization startupInitialization
    ) : ControllerBase
    {
        [HttpGet("{id}")]
        [EndpointSummary("Get user by ID")]
        [EndpointDescription("Returns a user by their unique identifier.")]
        public async Task<IActionResult> GetUserAsync(
            string id,
            CancellationToken cancellationToken
        )
        {
            logger
                .LogInformation(
                    "Received user request for id: {id}",
                    id
                );

            if (!ObjectId
                    .TryParse(
                        id,
                        out _
                    )
               )
            {
                // invalid id = assume user would not exist
                throw new RocketException(
                    $"User id: {id} not found",
                    ApiStatusCodeEnum.UnknownUser
                );
            }

            var user =
                await
                    userManager
                        .GetUserByUserIdAsync(
                            id,
                            cancellationToken
                        );

            if (user == null)
            {
                throw new RocketException(
                    $"User id: {id} not found",
                    ApiStatusCodeEnum.UnknownUser
                );
            }

            var response =
                new UserDetail
                {
                    Id = user.Id,
                    Username = user.Username,
                    CreatedAt = user.CreatedAt.ToLocalTime(),
                    LastLoginAt = user.LastLoginAt?.ToLocalTime(),
                    IsActive = user.IsActive,
                    IsAdmin = user.IsAdmin
                };

            return
                response
                    .AsApiSuccess();
        }

        [HttpPost("create")]
        [EndpointSummary("Add a new user")]
        [EndpointDescription(
            """
            Creates a new system user. If this is the first user created by the `admin` account, 
            then on success, the administrator account will be made inactive.
            """
        )]
        public async Task<IActionResult> CreateUserAsync(
            [FromBody] CreateUserRequest request,
            CancellationToken cancellationToken
        )
        {
            logger
                .LogInformation(
                    "Received user creation request for username: {username}",
                    request.Username
                );

            // Check if this is first start (admin is creating first user)
            var currentUsername =
                User
                    .Identity?
                    .Name;

            var newUserIsAdmin =
                currentUsername == DomainConstants.AdminUserName ||
                request.IsTheNewAdmin;

            // Create the new user account
            var newUser =
                await
                    userManager
                        .CreateUserAccountAsync(
                            request.Username,
                            request.Password,
                            newUserIsAdmin,
                            cancellationToken
                        );

            if (currentUsername == DomainConstants.AdminUserName)
            {
                var startupPhase =
                    await
                        startupInitialization
                            .GetStartupPhaseAsync(cancellationToken);

                if (startupPhase == StartupPhaseEnum.AdminPendingDeactivation)
                {
                    logger
                        .LogInformation("First user created by admin. Deactivating admin account.");

                    await
                        userManager
                            .DeactivateAdminAccountAsync(cancellationToken);
                }
            }
            else
            {
                if (newUserIsAdmin)
                {
                    var userId =
                        User
                            .FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?
                            .Value;

                    // set this user = not admin
                    await
                        userManager
                            .UpdateAccountIsAdminAsync(
                                userId,
                                false,
                                cancellationToken
                            );
                }
            }

            var response =
                new CreateUserResponse
                {
                    Username = newUser.Username,
                    CreatedAt = newUser.CreatedAt
                };

            return
                response
                    .AsApiSuccess();
        }

        [HttpPost("update")]
        [EndpointSummary("Update an existing user")]
        [EndpointDescription(
            """
            Updates one or more details of an existing system user. A value not supplied will not be updated.
            If the update sets the `IsAdmin` flag to true, then the user calling the API will have their administrator status removed.
            """
        )]
        public async Task<IActionResult> UpdateUserAsync(
            [FromBody] UserDetail request,
            CancellationToken cancellationToken
        )
        {
            logger
                .LogInformation(
                    "Received user update request for username: {id}",
                    request.Id
                );

            var newUserIsAdmin =
                request
                    .IsAdmin;

            // Update the user account
            await
                userManager
                    .UpdateAccountAsync(
                        request.Id,
                        request.Username,
                        request.IsActive,
                        request.IsAdmin,
                        request.NewPassword,
                        cancellationToken
                    );

            if (newUserIsAdmin.GetValueOrDefault())
            {
                var userId =
                    User
                        .FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?
                        .Value;

                // set this user => not admin
                await
                    userManager
                        .UpdateAccountIsAdminAsync(
                            userId,
                            false,
                            cancellationToken
                        );
            }

            var response =
                new UpdateUserResponse();

            return
                response
                    .AsApiSuccess();
        }
    }
}