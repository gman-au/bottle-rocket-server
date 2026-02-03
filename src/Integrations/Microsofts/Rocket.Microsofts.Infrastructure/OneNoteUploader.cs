using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rocket.Microsofts.Domain;

namespace Rocket.Microsofts.Infrastructure
{
    public class OneNoteUploader(
        IGraphClientProvider graphClientProvider,
        IMicrosoftTokenAcquirer tokenAcquirer
    ) : IOneNoteUploader
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
            var accessToken =
                await
                    tokenAcquirer
                        .AcquireTokenSilentAsync(
                            connector,
                            cancellationToken
                        );

            var imageFileName = $"{pageTitle}{fileExtension}";

            var htmlContent = $"""
                               <!DOCTYPE html>
                               <html>
                               <head>
                                   <title>{pageTitle}</title>
                               </head>
                               <body>
                                   <img src="name:{imageFileName}" alt={imageFileName}" />
                               </body>
                               </html>
                               """;

            using var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(
                    "Bearer",
                    accessToken
                );

            using var formContent = new MultipartFormDataContent();

            var htmlPart =
                new StringContent(
                    htmlContent,
                    Encoding.UTF8,
                    "text/html"
                );

            formContent
                .Add(
                    htmlPart,
                    "Presentation"
                );

            var imagePart =
                new ByteArrayContent(imageBytes);

            imagePart.Headers.ContentType =
                new MediaTypeHeaderValue($"image/{fileExtension.Replace(".", "")}");

            formContent
                .Add(
                    imagePart,
                    imageFileName
                );

            var response =
                await
                    httpClient
                        .PostAsync(
                            $"https://graph.microsoft.com/v1.0/me/onenote/sections/{sectionId}/pages",
                            formContent,
                            cancellationToken
                        );

            response
                .EnsureSuccessStatusCode();
        }
    }
}