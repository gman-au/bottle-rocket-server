using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
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
        IUserRepository userRepository,
        IStartupInitialization startupInitialization,
        IActiveAdminChecker activeAdminChecker
    ) : RocketControllerBase(userManager)
    {
        [HttpPost("fetch")]
        [EndpointSummary("Fetch the system users")]
        [EndpointGroupName("Manage users")]
        [EndpointDescription(
            """
            Retrieves a subset of users belonging to the system.\n
            Provide a zero-based start index and record count to retrieve paged results and minimise server load. 
            """
        )]
        [ProducesResponseType(typeof(FetchUsersResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> FetchUsersAsync(
            [FromBody] FetchUsersRequest request,
            CancellationToken cancellationToken
        )
        {
            await
                ThrowIfNotAdminAsync(cancellationToken);

            var (records, totalRecordCount) =
                await
                    userRepository
                        .FetchUsersAsync(
                            request.StartIndex,
                            request.RecordCount,
                            cancellationToken
                        );

            var response =
                new FetchUsersResponse
                {
                    Users =
                        records
                            .Select(
                                o =>
                                    new UserItem
                                    {
                                        Id = o.Username == DomainConstants.RootAdminUserName ? null : o.Id,
                                        Username = o.Username,
                                        CreatedAt = o.CreatedAt.ToLocalTime(),

                                    }
                            ),
                    TotalRecords = (int)totalRecordCount
                };

            return
                response
                    .AsApiSuccess();
        }
        
        [HttpGet("get/{id}")]
        [EndpointSummary("Get user by ID")]
        [EndpointGroupName("Manage users")]
        [EndpointDescription("Returns a user by their unique identifier.")]
        [ProducesResponseType(typeof(UserDetail), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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

            await
                ThrowIfNotAdminAsync(cancellationToken);

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
                    UserManager
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
        [EndpointGroupName("Manage users")]
        [EndpointDescription(
            """
            Creates a new system user. If this is the first user created by the `admin` account, 
            then on success, the administrator account will be made inactive.
            """
        )]
        [ProducesResponseType(typeof(CreateUserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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

            var currentUser =
                await
                    ThrowIfNotAdminAsync(cancellationToken);

            var currentUserId =
                currentUser
                    .Id;

            // Check if this is first start (admin is creating first user)
            var currentUsername =
                currentUser
                    .Username;

            var newUserIsAdmin =
                currentUsername == DomainConstants.RootAdminUserName ||
                request.IsTheNewAdmin;

            // Create the new user account
            var newUser =
                await
                    UserManager
                        .CreateUserAccountAsync(
                            request.Username,
                            request.Password,
                            newUserIsAdmin,
                            cancellationToken
                        );

            if (newUser == null)
                throw new RocketException(
                    "There was an error writing the user record",
                    ApiStatusCodeEnum.ServerError
                );

            if (currentUsername == DomainConstants.RootAdminUserName)
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
                        UserManager
                            .DeactivateAdminAccountAsync(cancellationToken);
                }
            }
            else
            {
                if (newUserIsAdmin)
                {
                    // set this user = not admin
                    await
                        UserManager
                            .UpdateAccountIsAdminAsync(
                                currentUserId,
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
        [EndpointGroupName("Manage users")]
        [EndpointDescription(
            """
            Updates one or more details of an existing system user. A value not supplied will not be updated.\n
            If the update sets the `IsAdmin` flag to true, then the user calling the API will have their administrator status removed.
            """
        )]
        [ProducesResponseType(typeof(UpdateUserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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

            await
                ThrowIfNotAdminAsync(cancellationToken);

            var newUserIsAdmin =
                request
                    .IsAdmin;

            var isPermissibleOperation =
                await
                    activeAdminChecker
                        .PerformAsync(
                            request.Id,
                            request.IsActive,
                            request.IsAdmin,
                            cancellationToken
                        );

            if (!isPermissibleOperation)
                throw new RocketException(
                    "There must be at least one active admin user in the system.",
                    ApiStatusCodeEnum.PotentiallyIrrecoverableOperation
                );
            
            // Update the user account
            await
                UserManager
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
                    UserManager
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