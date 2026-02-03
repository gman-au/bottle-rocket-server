using System;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Rocket.Gcp.Domain;

namespace Rocket.Gcp.Infrastructure
{
    public class DriveUploadService : IDriveUploadService
    {
        public async Task UploadFileAsync(
            byte[] fileBytes,
            string fileExtension,
            GcpCredential gcpCredential,
            CancellationToken cancellationToken
        )
        {
            // Convert your credential object back to JSON string
            var credentialJson =
                JsonSerializer
                    .Serialize(gcpCredential);

            // Create GoogleCredential from JSON content (new way)
            GoogleCredential credential;
            using (var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(credentialJson)))
            {
                credential =
                    await
                        GoogleCredential
                            .FromStreamAsync(
                                stream,
                                cancellationToken
                            );
            }

            var service =
                new DriveService(
                    new BaseClientService.Initializer
                    {
                        HttpClientInitializer = credential
                    }
                );

            var fileName = $"{Guid.NewGuid()}{fileExtension}";

            var fileMetadata = new Google.Apis.Drive.v3.Data.File
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