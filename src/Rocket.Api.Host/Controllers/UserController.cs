using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Rocket.Api.Contracts;
using Rocket.Api.Host.Extensions;
using Rocket.Domain.Enum;
using Rocket.Domain.Utils;
using Rocket.Interfaces;

namespace Rocket.Api.Host.Controllers
{
    [ApiController]
    [Route("/api/user")]
    [Authorize]
    public class UserController(
        ILogger<UserController> logger,
        IUserManager userManager,
        IStartupInitialization startupInitialization
    ) : ControllerBase
    {
        [HttpPost("create")]
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

            // Create the new user account
            var newUser =
                await
                    userManager
                        .CreateUserAccountAsync(
                            request.Username,
                            request.Password,
                            cancellationToken
                        );

            // Check if this is first start (admin is creating first user)
            var currentUsername =
                User
                    .Identity?
                    .Name;

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
    }
}