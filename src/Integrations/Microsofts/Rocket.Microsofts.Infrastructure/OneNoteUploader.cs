using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Graph;
using Rocket.Microsofts.Domain;

namespace Rocket.Microsofts.Infrastructure
{
    public class OneNoteUploader(IGraphClientProvider graphClientProvider) : IOneNoteUploader
    {
        public async Task UploadNoteAsync(
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

            // Create simple HTML content
            var htmlContent = $@"
<!DOCTYPE html>
<html>
<head>
    <title>{"BR PAGE TITLE"}</title>
</head>
<body>
    <h1>{"BR TITLE"}</h1>
    <p>{"BR TEXT CONTENT"}</p>
</body>
</html>";

            using Stream contentStream = new MemoryStream(Encoding.UTF8.GetBytes(htmlContent));

            await graphClient
                .Me
                .Onenote
                .Sections["BR SECTION"]
                .Pages
                .Request()
                .AddAsync(
                    contentStream,
                    "html",
                    cancellationToken
                );
        }
    }
}