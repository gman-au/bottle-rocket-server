using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rocket.Microsofts.Domain;

namespace Rocket.Microsofts.Infrastructure
{
    public class OneNoteUploader(IGraphClientProvider graphClientProvider) : IOneNoteUploader
    {
        private const string HtmlContentType = "text/html";

        public async Task UploadTextNoteAsync(
            MicrosoftConnector connector,
            string sectionId,
            string pageTitle,
            string textContent,
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
            var htmlContent = $"""
                                   <!DOCTYPE html>
                                   <html>
                                   <head>
                                       <title>{pageTitle}</title>
                                   </head>
                                   <body>
                                       <h1>{pageTitle}</h1>
                                       <p>{textContent}</p>
                                   </body>
                                   </html>
                               """;

            await using Stream contentStream = new MemoryStream(Encoding.UTF8.GetBytes(htmlContent));

            await
                graphClient
                    .Me
                    .Onenote
                    .Sections[sectionId]
                    .Pages
                    .Request()
                    .AddAsync(
                        contentStream,
                        HtmlContentType,
                        cancellationToken
                    );
        }

        public async Task UploadImageNoteAsync(
            MicrosoftConnector connector,
            string sectionId,
            string fileExtension,
            string pageTitle,
            byte[] imageBytes,
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

            // Convert to base64
            var base64Image =
                Convert
                    .ToBase64String(imageBytes);

            var dataUri = $"data:{fileExtension?.Replace(".", "")};base64,{base64Image}";

            // Create HTML with embedded base64 image
            var htmlContent = $"""
                                   <!DOCTYPE html>
                                   <html>
                                   <head>
                                       <title>{pageTitle}</title>
                                   </head>
                                   <body>
                                       <img src="{dataUri}" alt="{pageTitle}" />
                                   </body>
                                   </html>
                               """;

            await using Stream contentStream = new MemoryStream(Encoding.UTF8.GetBytes(htmlContent));

            await
                graphClient
                    .Me
                    .Onenote
                    .Sections[sectionId]
                    .Pages
                    .Request()
                    .AddAsync(
                        contentStream,
                        HtmlContentType,
                        cancellationToken
                    );
        }
    }
}