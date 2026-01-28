using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Graph;
using Rocket.Microsofts.Domain;

namespace Rocket.Microsofts.Infrastructure
{
    public class OneDriveUploader(IMicrosoftTokenAcquirer microsoftTokenAcquirer)
        : IOneDriveUploader
    {
        public async Task UploadFileAsync(
            MicrosoftConnector connector,
            string fileName,
            string folderPath,
            byte[] inputBytes,
            CancellationToken cancellationToken
        )
        {
            var accessToken =
                await
                    microsoftTokenAcquirer
                        .AcquireTokenSilentAsync(
                            connector,
                            cancellationToken
                        );

            var authProvider =
                new DelegateAuthenticationProvider(
                    requestMessage =>
                    {
                        requestMessage.Headers.Authorization =
                            new
                                System.Net.Http.Headers.AuthenticationHeaderValue(
                                    "Bearer",
                                    accessToken
                                );

                        return
                            Task
                                .CompletedTask;
                    }
                );

            var graphClient =
                new GraphServiceClient(authProvider);

            var fullFilePath =
                $"{folderPath}{(!folderPath.EndsWith('/') ? "/" : "")}{fileName}";

            using var fileStream =
                new MemoryStream(inputBytes);

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
        }
    }
}