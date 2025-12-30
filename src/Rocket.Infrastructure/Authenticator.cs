using System;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Logging;
using Rocket.Domain;
using Rocket.Interfaces;

namespace Rocket.Infrastructure
{
    public class Authenticator(
        ILogger<Authenticator> logger,
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

                if (!VerifyPassword(
                        password,
                        user.PasswordHash
                    ))
                {
                    logger.LogWarning(
                        "Authentication failed: Invalid password for user {username}",
                        username
                    );
                    return null;
                }

                // Update last login
                await
                    userRepository
                        .UpdateLastLoginAsync(
                            user.Id,
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

        public string HashPassword(string password)
        {
            // Generate a 128-bit salt using a secure PRNG
            var salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            // Hash the password with PBKDF2
            var hashed =
                Convert
                    .ToBase64String(
                        KeyDerivation.Pbkdf2(
                            password: password,
                            salt: salt,
                            prf: KeyDerivationPrf.HMACSHA256,
                            iterationCount: 10000,
                            numBytesRequested: 256 / 8
                        )
                    );

            // Return salt + hash
            return $"{Convert.ToBase64String(salt)}.{hashed}";
        }

        public bool VerifyPassword(string password, string passwordHash)
        {
            try
            {
                var parts =
                    passwordHash
                        .Split('.');

                if (parts.Length != 2)
                    return false;

                var salt =
                    Convert
                        .FromBase64String(parts[0]);

                var hash = parts[1];

                // Hash the input password with the stored salt
                var inputHash =
                    Convert
                        .ToBase64String(
                            KeyDerivation
                                .Pbkdf2(
                                    password: password,
                                    salt: salt,
                                    prf: KeyDerivationPrf.HMACSHA256,
                                    iterationCount: 10000,
                                    numBytesRequested: 256 / 8
                                )
                        );

                return inputHash == hash;
            }
            catch
            {
                return false;
            }
        }
    }
}