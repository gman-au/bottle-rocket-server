using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Rocket.Domain.Enum;
using Rocket.Domain.Exceptions;
using Rocket.Infrastructure.Blob.Local.Options;
using Rocket.Interfaces;

namespace Rocket.Infrastructure.Blob.Local
{
    public class LocalBlobStore(
        ILogger<LocalBlobStore> logger,
        IOptions<LocalBlobConfigurationOptions> optionsAccessor
    ) : IBlobStore
    {
        private readonly LocalBlobConfigurationOptions _options = optionsAccessor.Value;

        public async Task<string> SaveImageAsync(
            byte[] imageData,
            string fileExtension,
            CancellationToken cancellationToken
        )
        {
            try
            {
                var localBasePath =
                    _options.LocalBasePath ??
                    throw new ConfigurationErrorsException(
                        nameof(LocalBlobConfigurationOptions.LocalBasePath)
                    );

                var localSubfolder =
                    _options.LocalSubfolder ??
                    throw new ConfigurationErrorsException(
                        nameof(LocalBlobConfigurationOptions.LocalSubfolder)
                    );

                var blobId =
                    Guid
                        .NewGuid()
                        .ToString();

                using var ms = new MemoryStream();

                var folderPath =
                    Path
                        .Combine(
                            localBasePath,
                            localSubfolder,
                            Path.GetDirectoryName(blobId)
                        );

                EnsureFolderExists(folderPath);

                var fullFilePath = $"{Path.GetFileName(blobId)}{fileExtension}";

                var sourcePath =
                    Path
                        .Combine(
                            folderPath,
                            fullFilePath
                        );

                logger
                    .LogInformation(
                        "Using Local File storage path {path}",
                        sourcePath
                    );

                await
                    File
                        .WriteAllBytesAsync(
                            sourcePath,
                            imageData,
                            cancellationToken
                        );

                return blobId;
            }
            catch (ConfigurationErrorsException ex)
            {
                throw new RocketException(
                    "The server requires configuration of local storage.",
                    ApiStatusCodeEnum.ServerConfigurationError,
                    (int)HttpStatusCode.InternalServerError,
                    ex
                );
            }
        }

        public async Task<byte[]> LoadImageAsync(
            string filePath,
            string fileExtension,
            CancellationToken cancellationToken
        )
        {
            var folderPath =
                Path
                    .Combine(
                        _options.LocalBasePath,
                        _options.LocalSubfolder,
                        Path.GetDirectoryName(filePath)
                    );

            var fullFilePath = $"{Path.GetFileName(filePath)}{fileExtension}";

            var sourcePath =
                Path
                    .Combine(
                        folderPath,
                        fullFilePath
                    );

            logger
                .LogInformation(
                    "Using Local File storage path {path}",
                    sourcePath
                );

            try
            {
                var imageData =
                    await
                        File
                            .ReadAllBytesAsync(
                                sourcePath,
                                cancellationToken
                            );

                return imageData;
            }
            catch (FileNotFoundException)
            {
                logger
                    .LogError(
                        "File not found at path {path}",
                        sourcePath
                    );

                return [];
            }
        }

        public async Task<bool> DeleteImageAsync(
            string blobId,
            string fileExtension,
            CancellationToken cancellationToken)
        {
            var filePath = $"{Path.GetFileName(blobId)}{fileExtension}";

            var fullFilePath =
                Path
                    .Combine(
                        _options.LocalBasePath,
                        _options.LocalSubfolder,
                        filePath
                    );

            logger
                .LogInformation(
                    "Using Local File storage path {path}",
                    fullFilePath
                );

            if (!File.Exists(fullFilePath))
                throw new RocketException(
                    $"Could not find file to delete at path {fullFilePath}",
                    ApiStatusCodeEnum.UnknownOrInaccessibleRecord
                );

            File
                .Delete(fullFilePath);

            return true;
        }

        private static void EnsureFolderExists(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }
    }
}