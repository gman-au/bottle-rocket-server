using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Graph;
using Rocket.Microsofts.Domain;

namespace Rocket.Microsofts.Infrastructure
{
    public class OneDriveUploader(IGraphClientProvider graphClientProvider)
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
            var graphClient =
                await
                    graphClientProvider
                        .GetClientAsync(
                            connector,
                            cancellationToken
                        );

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