using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Rocket.Interfaces;

namespace Rocket.Infrastructure.Auth
{
    public class MasterKey : IMasterKey, IAppHealthCheck
    {
        private const string MasterKeyEnvVar = "BOTTLE_ROCKET_MASTER_KEY";
        private const string MasterKeyPathEnvVar = "BOTTLE_ROCKET_KEY_PATH";
        
        private readonly IConfiguration _configuration;
        private byte[] _key;

        public MasterKey(
            IConfiguration configuration
        )
        {
            _configuration = configuration;
            InitializeKeyAndPath();
        }

        public async Task PreRunAsync()
        {
        }

        public byte[] Get() => _key;

        private void InitializeKeyAndPath()
        {
            var authSecret = _configuration[MasterKeyEnvVar];
            var keyFilePath = _configuration[MasterKeyPathEnvVar];

            _key =
                LoadOrCreate(
                    authSecret,
                    keyFilePath
                );
        }

        private static byte[] LoadOrCreate(
            string authSecret,
            string keyFilePath
        )
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(authSecret))
                    return
                        Validate(
                            Convert
                                .FromBase64String(
                                    authSecret
                                        .Trim()
                                )
                        );
            }
            catch (FormatException ex)
            {
                throw new Exception($"Could not load master key specified in {MasterKeyEnvVar}: {ex.Message}");
            }

            try
            {
                if (File.Exists(keyFilePath))
                    return
                        Validate(
                            Convert
                                .FromBase64String(
                                    File
                                        .ReadAllText(keyFilePath)
                                        .Trim()
                                )
                        );
            }
            catch (FormatException ex)
            {
                throw new Exception($"Could not load master key from file path {keyFilePath}: {ex.Message}");
            }

            var key =
                RandomNumberGenerator
                    .GetBytes(32);

            Directory
                .CreateDirectory(
                    Path
                        .GetDirectoryName(
                            Path
                                .GetFullPath(
                                    keyFilePath
                                )
                        )!
                );

            try
            {
                using var fs =
                    new FileStream(
                        keyFilePath,
                        FileMode.CreateNew,
                        FileAccess.Write
                    );

                fs
                    .Write(
                        System
                            .Text
                            .Encoding
                            .ASCII
                            .GetBytes(
                                Convert.ToBase64String(key)
                            )
                    );
            }
            catch (IOException)
            {
                return
                    Validate(
                        Convert
                            .FromBase64String(
                                File
                                    .ReadAllText(keyFilePath)
                                    .Trim()
                            )
                    );
            }

            if (!OperatingSystem.IsWindows())
                File
                    .SetUnixFileMode(
                        keyFilePath,
                        UnixFileMode.UserRead | UnixFileMode.UserWrite
                    );

            return key;
        }

        private static byte[] Validate(
            byte[] key
        ) =>
            key.Length == 32
                ? key
                : throw new InvalidOperationException("Master key must be exactly 32 bytes (base64-encoded).");
    }
}