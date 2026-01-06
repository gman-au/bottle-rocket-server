using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Rocket.Domain;
using Rocket.Interfaces;

namespace Rocket.Infrastructure
{
    public class Authenticator(
        ILogger<Authenticator> logger,
        IPasswordHasher passwordHasher,
        IUserRepository userRepository
    ) : IAuthenticator
    {
        public async Task<User> AuthenticateAsync(
            string username,
            string password,
            CancellationToken cancellationToken
        )
        {
            try
            {
                var user =
                    await
                        userRepository
                            .GetUserByUsernameAsync(
                                username,
                                cancellationToken
                            );

                if (user == null)
                {
                    logger
                        .LogWarning(
                            "Authentication failed: User {username} not found",
                            username
                        );
                    return null;
                }

                if (!user.IsActive)
                {
                    logger
                        .LogWarning(
                            "Authentication failed: User {username} is inactive",
                            username
                        );
                    return null;
                }

                if (!passwordHasher.VerifyPassword(
                        password,
                        user.PasswordHash
                    ))
                {
                    logger
                        .LogWarning(
                            "Authentication failed: Invalid password for user {username}",
                            username
                        );
                    return null;
                }

                // Update last login
                await
                    userRepository
                        .UpdateUserFieldAsync(
                            user.Id,
                            o => o.LastLoginAt,
                            DateTime.UtcNow,
                            cancellationToken
                        );

                logger
                    .LogInformation(
                        "User {username} authenticated successfully",
                        username
                    );
                return user;
            }
            catch (Exception ex)
            {
                logger
                    .LogError(
                        ex,
                        "Error during authentication for user {username}",
                        username
                    );
                throw;
            }
        }
    }
}