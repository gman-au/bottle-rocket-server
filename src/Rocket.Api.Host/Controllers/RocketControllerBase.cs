using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Rocket.Domain;
using Rocket.Domain.Enum;
using Rocket.Domain.Exceptions;
using Rocket.Interfaces;

namespace Rocket.Api.Host.Controllers
{
    public class RocketControllerBase(IUserManager userManager) : ControllerBase
    {
        protected readonly IUserManager UserManager = userManager;

        protected async Task<User> ThrowIfNotAdminAsync(CancellationToken cancellationToken)
        {
            var user =
                await
                    ThrowIfNotActiveUserAsync(cancellationToken);

            if (!user.IsAdmin)
            {
                throw new RocketException(
                    "This operation requires the logged in user to be an administrator.",
                    ApiStatusCodeEnum.RequiresAdministratorAccess
                );
            }

            return user;
        }

        protected async Task<User> ThrowIfNotActiveUserAsync(CancellationToken cancellationToken)
        {
            var user =
                await
                    GetLoggedInUserAsync(cancellationToken);
            
            if (user == null)
            {
                throw new RocketException(
                    "Authenticated attempt from unknown user",
                    ApiStatusCodeEnum.UnknownUser,
                    (int)HttpStatusCode.Unauthorized
                );
            }

            if (!user.IsActive)
            {
                throw new RocketException(
                    "User account is inactive. Please contact your administrator.",
                    ApiStatusCodeEnum.InactiveUser
                );
            }

            return user;
        }

        protected async Task<User> GetLoggedInUserAsync(CancellationToken cancellationToken)
        {
            var userId =
                User
                    .FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?
                    .Value;

            return
                await
                    UserManager
                        .GetUserByUserIdAsync(
                            userId,
                            cancellationToken
                        );
        }
    }
}