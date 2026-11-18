using System.Threading;
using System.Threading.Tasks;
using Rocket.Domain.Enum;
using Rocket.Domain.Exceptions;
using Rocket.Interfaces;

namespace Rocket.Markdig.Infrastructure
{
    public class AuthorRetriever(
        IUserRepository userRepository
    ) : IAuthorRetriever
    {
        public async Task<string> RetrieveUserNameAsync(
            string userId,
            CancellationToken cancellationToken
        )
        {
            var user =
                await
                    userRepository
                        .GetUserByIdAsync(
                            userId,
                            cancellationToken
                        );

            if (user == null)
            {
                throw new RocketException(
                    $"User id: {userId} not found",
                    ApiStatusCodeEnum.UnknownUser
                );
            }

            return
                user
                    .Username;
        }
    }
}