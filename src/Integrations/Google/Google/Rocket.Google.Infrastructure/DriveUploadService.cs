using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Rocket.Domain.Enum;
using Rocket.Domain.Exceptions;
using Rocket.Domain.Jobs;
using Rocket.Google.Domain;
using Rocket.Integrations.Common.Extensions;
using Rocket.Interfaces;
using File = Google.Apis.Drive.v3.Data.File;

namespace Rocket.Google.Infrastructure
{
    public class DriveUploadService(
        IGoogleTokenAcquirer tokenAcquirer
    ) : IDriveUploadService
    {
        public async Task UploadFileAsync(
            IJobControl jobControl,
            IEnumerable<ArtifactDataElement> elements,
            string targetFolderId,
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

            var allCompleted = true;
            
            foreach (var element in elements)
            {
                try
                {
                    await
                        jobControl
                            .StartStepAsync(
                                element.ScanId
                            );

                    var fullFileName = $"{element.FileName}{element.FileExtension}";

                    var fileMetadata =
                        new File
                        {
                            Name = fullFileName,
                            Parents = new List<string> { targetFolderId }
                        };

                    await using (var fileStream = new MemoryStream(element.Artifact))
                    {
                        var mimeType = $"image/{element.FileExtension.Replace(".", "")}";

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

                        var progress =
                            await
                                request
                                    .UploadAsync(cancellationToken);
                        
                        if (progress.Exception != null)
                            throw progress.Exception;
                        
                        await
                            jobControl
                                .SucceedStepAsync(
                                    $"File {element.FileName} uploaded to Google Drive, {progress.BytesSent} bytes sent",
                                    element.ScanId
                                );
                    }
                }
                catch (Exception ex)
                {
                    allCompleted = false;

                    await
                        jobControl
                            .FailStepAsync(
                                ex,
                                element.ScanId
                            );
                }
            }

            if (!allCompleted)
            {
                throw new RocketException(
                    "There was an error uploading the data to Google Drive",
                    ApiStatusCodeEnum.ThirdPartyServiceError
                );
            }
        }
    }
}