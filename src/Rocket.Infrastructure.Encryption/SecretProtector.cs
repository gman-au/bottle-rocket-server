using System;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Logging;
using Rocket.Domain.Utils;
using Rocket.Interfaces;

namespace Rocket.Infrastructure.Encryption
{
    public class SecretProtector(
        IMasterKey masterKey,
        ILogger<SecretProtector> logger
    ) : ISecretProtector
    {
        private const byte Version = 1;
        private const int NonceSize = 12;
        private const int TagSize = 16;

        private readonly byte[] _key = masterKey.Get();

        public string Protect(
            string plaintext
        )
        {
            var data =
                Encoding
                    .UTF8
                    .GetBytes(plaintext);

            var nonce =
                RandomNumberGenerator
                    .GetBytes(NonceSize);

            var cipher = new byte[data.Length];
            var tag = new byte[TagSize];

            using (var aes =
                   new AesGcm(
                       _key,
                       TagSize
                   ))
                aes
                    .Encrypt(
                        nonce,
                        data,
                        cipher,
                        tag
                    );

            // layout: [version][nonce][tag][ciphertext]
            var result = new byte[1 + NonceSize + TagSize + cipher.Length];
            result[0] = Version;

            nonce
                .CopyTo(
                    result,
                    1
                );
            tag
                .CopyTo(
                    result,
                    1 + NonceSize
                );
            cipher
                .CopyTo(
                    result,
                    1 + NonceSize + TagSize
                );

            return
                Convert
                    .ToBase64String(result);
        }

        public string Unprotect(
            string protectedValue
        )
        {
            byte[] plain;

            try
            {
                var raw =
                    Convert
                        .FromBase64String(protectedValue);

                if (raw.Length < 1 + NonceSize + TagSize || raw[0] != Version)
                    throw new CryptographicException("Unrecognised encrypted value format.");

                var nonce =
                    raw
                        .AsSpan(
                            1,
                            NonceSize
                        );

                var tag =
                    raw
                        .AsSpan(
                            1 + NonceSize,
                            TagSize
                        );

                var cipher =
                    raw
                        .AsSpan(1 + NonceSize + TagSize);

                plain = new byte[cipher.Length];

                using var aes = new AesGcm(
                    _key,
                    TagSize
                );

                aes
                    .Decrypt(
                        nonce,
                        cipher,
                        tag,
                        plain
                    );
            }
            catch (CryptographicException)
            {
                logger
                    .LogError(
                        "Failed to decrypt a stored credential. The master key is probably different from the one used to encrypt it."
                    );

                return
                    DomainConstants
                        .ErrorInProperty;
            }
            catch (FormatException)
            {
                logger
                    .LogError(
                        "Failed to decrypt a stored credential. The stored credential is likely retained from a previous release version, and should be recreated."
                    );

                return
                    DomainConstants
                        .ErrorInProperty;
            }

            return
                Encoding
                    .UTF8
                    .GetString(plain);
        }
    }
}