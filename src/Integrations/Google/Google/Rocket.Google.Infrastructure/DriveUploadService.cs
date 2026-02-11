using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Rocket.Domain.Enum;
using Rocket.Domain.Exceptions;
using Rocket.Google.Domain;
using File = Google.Apis.Drive.v3.Data.File;

namespace Rocket.Google.Infrastructure
{
    public class DriveUploadService(IGoogleTokenAcquirer tokenAcquirer) : IDriveUploadService
    {
        public async Task UploadFileAsync(
            byte[] fileBytes,
            string fileExtension,
            GoogleConnector googleConnector,
            CancellationToken cancellationToken
        )
        {
            var googlesCredential =
                googleConnector
                    .Credential;

            if (googlesCredential == null)
                throw new RocketException(
                    "Credential file is not supplied for operation, please check Google connector",
                    ApiStatusCodeEnum.ThirdPartyServiceError
                );

            var refreshToken =
                googleConnector
                    .RefreshToken;

            if (string.IsNullOrWhiteSpace(refreshToken))
                throw new RocketException(
                    "Refresh token is not supplied for operation, please check Google connector",
                    ApiStatusCodeEnum.ThirdPartyServiceError
                );
            
            var credential =
                await tokenAcquirer
                    .GetFlowCredentialAsync(
                        googlesCredential,
                        refreshToken,
                        cancellationToken
                    );

            var service =
                new DriveService(
                    new BaseClientService.Initializer
                    {
                        HttpClientInitializer = credential
                    }
                );

            var fileName = $"{Guid.NewGuid()}{fileExtension}";

            var fileMetadata = new File
            {
                Name = fileName,
                // Parents = new List<string> { TargetFolderId } // Specify the parent folder
            };

            await using (var fileStream = new MemoryStream(fileBytes))
            {
                var mimeType = $"image/{fileExtension.Replace(".", "")}";

                // Create a new file, with metadata and stream
                var request =
                    service
                        .Files
                        .Create(
                            fileMetadata,
                            fileStream,
                            mimeType
                        );

                request.Fields = "*"; // Request all fields in the response

                await
                    request
                        .UploadAsync(cancellationToken);
            }
        }
    }
}