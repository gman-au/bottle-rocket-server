using System;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Rocket.Interfaces;

namespace Rocket.Infrastructure.Hashing
{
    public class PasswordHasher : IPasswordHasher
    {
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
                        KeyDerivation
                            .Pbkdf2(
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