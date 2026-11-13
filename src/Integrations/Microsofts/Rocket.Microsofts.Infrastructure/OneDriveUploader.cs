using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Graph;
using Rocket.Domain.Enum;
using Rocket.Domain.Exceptions;
using Rocket.Domain.Jobs;
using Rocket.Integrations.Common.Extensions;
using Rocket.Interfaces;
using Rocket.Microsofts.Domain;

namespace Rocket.Microsofts.Infrastructure
{
    public class OneDriveUploader(
        IGraphClientProvider graphClientProvider
    )
        : IOneDriveUploader
    {
        public async Task UploadFileAsync(
            MicrosoftConnector connector,
            IJobControl jobControl,
            IEnumerable<ArtifactDataElement> elements,
            string folderPath,
            CancellationToken cancellationToken
        )
        {
            var graphClient =
                await
                    graphClientProvider
                        .GetClientAsync(
                            connector,
                            cancellationToken
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

                    var noteTitle = element.FileName ?? $"BR_Note_{DateTime.Now:yyyy_MM_dd_HH_mm_ss}";
                    var fileName = noteTitle + element.FileExtension;

                    var fullFilePath =
                        $"{folderPath}{(!folderPath.EndsWith('/') ? "/" : "")}{fileName}";

                    using var fileStream =
                        new MemoryStream(element.Artifact);

                    var driveItem =
                        await
                            graphClient
                                .Me
                                .Drive
                                .Root
                                .ItemWithPath(fullFilePath)
                                .Content
                                .Request()
                                .PutAsync<DriveItem>(
                                    fileStream,
                                    cancellationToken
                                );

                    await
                        jobControl
                            .SucceedStepAsync(
                                $"File {element.FileName} uploaded to OneDrive {driveItem.Size}, bytes sent",
                                element.ScanId
                            );
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
                    "There was an error uploading the data to OneDrive",
                    ApiStatusCodeEnum.ThirdPartyServiceError
                );
            }
        }
    }
}