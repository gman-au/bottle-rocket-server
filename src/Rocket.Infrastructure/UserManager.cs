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
        IPasswordHasher passwordHasher,
        IEmailAddressValidator emailAddressValidator
    ) : IUserManager
    {
        public async Task<User> CreateUserAccountAsync(
            string userName,
            string password,
            bool isTheNewAdmin,
            CancellationToken cancellationToken
        )
        {
            try
            {
                await
                    ThrowIfUserNameExistsOrInvalidAsync(
                        userName,
                        cancellationToken
                    );

                ThrowIfPasswordInvalid(password);

                var passwordHash =
                    passwordHasher
                        .HashPassword(password);

                var newUser =
                    new User
                    {
                        Username = userName,
                        PasswordHash = passwordHash,
                        CreatedAt = DateTime.UtcNow,
                        IsAdmin = isTheNewAdmin,
                        IsActive = true
                    };

                await
                    userRepository
                        .InsertUserAsync(
                            newUser,
                            cancellationToken
                        );

                logger
                    .LogInformation(
                        "User account created: {username}",
                        userName
                    );

                return newUser;
            }
            catch (Exception ex)
            {
                logger
                    .LogError(
                        ex,
                        "Error creating user account for {username}",
                        userName
                    );
                throw;
            }
        }

        public async Task DeactivateAdminAccountAsync(CancellationToken cancellationToken) =>
            await
                userRepository
                    .DeactivateAdminUserAsync(cancellationToken);


        public async Task UpdateAccountIsAdminAsync(
            string userId,
            bool value,
            CancellationToken cancellationToken
        ) =>
            await
                userRepository
                    .UpdateUserFieldAsync(
                        userId,
                        o => o.IsAdmin,
                        value,
                        cancellationToken
                    );

        public async Task<User> GetUserByUserIdAsync(
            string userId,
            CancellationToken cancellationToken
        ) =>
            await
                userRepository
                    .GetUserByIdAsync(
                        userId,
                        cancellationToken
                    );

        public async Task UpdateAccountAsync(
            string userId,
            string userName,
            bool? isActive,
            bool? isAdmin,
            string newPassword,
            CancellationToken cancellationToken
        )
        {
            if (isActive.HasValue)
            {
                await
                    userRepository
                        .UpdateUserFieldAsync(
                            userId,
                            o => o.IsActive,
                            isActive.Value,
                            cancellationToken
                        );
            }

            if (isAdmin.HasValue)
            {
                await
                    userRepository
                        .UpdateUserFieldAsync(
                            userId,
                            o => o.IsAdmin,
                            isAdmin.Value,
                            cancellationToken
                        );
            }

            if (!string.IsNullOrEmpty(userName))
            {
                await
                    ThrowIfUserNameExistsOrInvalidAsync(
                        userName,
                        cancellationToken
                    );

                await
                    userRepository
                        .UpdateUserFieldAsync(
                            userId,
                            o => o.Username,
                            userName,
                            cancellationToken
                        );
            }

            if (!string.IsNullOrEmpty(newPassword))
            {
                ThrowIfPasswordInvalid(newPassword);

                var passwordHash =
                    passwordHasher
                        .HashPassword(newPassword);

                await
                    userRepository
                        .UpdateUserFieldAsync(
                            userId,
                            o => o.PasswordHash,
                            passwordHash,
                            cancellationToken
                        );
            }
        }

        private async Task ThrowIfUserNameExistsOrInvalidAsync(
            string userName,
            CancellationToken cancellationToken
        )
        {
            if (!emailAddressValidator.IsValid(userName))
            {
                throw
                    new RocketException(
                        "Invalid user name - please use an email address",
                        ApiStatusCodeEnum.InvalidUsername
                    );
            }

            var existingUser =
                await
                    userRepository
                        .GetUserByNameAsync(
                            userName,
                            cancellationToken
                        );

            if (existingUser != null)
            {
                throw new RocketException(
                    "Username already exists",
                    ApiStatusCodeEnum.UserAlreadyExists
                );
            }

            if (string.IsNullOrWhiteSpace(userName) || userName.Length < 3)
            {
                throw new RocketException(
                    "Username must be at least 3 characters",
                    ApiStatusCodeEnum.InvalidUsername
                );
            }
        }

        private void ThrowIfPasswordInvalid(string password)
        {
            // Validate password
            if (string.IsNullOrWhiteSpace(password) || password.Length < 8)
            {
                throw new RocketException(
                    "Password must be at least 8 characters",
                    ApiStatusCodeEnum.InvalidPassword
                );
            }
        }
    }
}