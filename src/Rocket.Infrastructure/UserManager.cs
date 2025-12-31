using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Rocket.Domain;
using Rocket.Domain.Enum;
using Rocket.Domain.Exceptions;
using Rocket.Interfaces;

namespace Rocket.Infrastructure
{
    public class UserManager(
        ILogger<UserManager> logger,
        IUserRepository userRepository,
        IPasswordHasher passwordHasher
    ) : IUserManager
    {
        public async Task<User> CreateUserAccountAsync(
            string username,
            string password,
            CancellationToken cancellationToken
        )
        {
            try
            {
                // Check if username already exists
                var existingUser =
                    await
                        userRepository
                            .GetUserByUsernameAsync(
                                username,
                                cancellationToken
                            );

                if (existingUser != null)
                {
                    throw new RocketException(
                        "Username already exists",
                        ApiStatusCodeEnum.UserAlreadyExists
                    );
                }

                // Validate username
                if (string.IsNullOrWhiteSpace(username) || username.Length < 3)
                {
                    throw new RocketException(
                        "Username must be at least 3 characters",
                        ApiStatusCodeEnum.InvalidUsername
                    );
                }

                // Validate password
                if (string.IsNullOrWhiteSpace(password) || password.Length < 8)
                {
                    throw new RocketException(
                        "Password must be at least 8 characters",
                        ApiStatusCodeEnum.InvalidPassword
                    );
                }

                var passwordHash =
                    passwordHasher
                        .HashPassword(password);

                var newUser =
                    new User
                    {
                        Username = username,
                        PasswordHash = passwordHash,
                        CreatedAt = DateTime.UtcNow,
                        IsActive = true
                    };

                await
                    userRepository
                        .CreateUserAsync(
                            newUser,
                            cancellationToken
                        );

                logger
                    .LogInformation(
                        "User account created: {username}",
                        username
                    );

                return newUser;
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "Error creating user account for {username}",
                    username
                );
                throw;
            }
        }

        public async Task DeactivateAdminAccountAsync(CancellationToken cancellationToken) =>
            await
                userRepository
                    .DeactivateAdminUserAsync(cancellationToken);
    }
}