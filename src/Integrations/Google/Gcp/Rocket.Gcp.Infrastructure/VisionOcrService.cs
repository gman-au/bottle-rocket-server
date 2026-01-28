using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Vision.V1;
using Rocket.Gcp.Domain;

namespace Rocket.Gcp.Infrastructure
{
    public class VisionOcrService : IVisionOcrService
    {
        public async Task<string> ExtractHandwrittenTextAsync(
            byte[] imageBytes,
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

            // Create client with specific credentials
            var clientBuilder =
                new ImageAnnotatorClientBuilder
                {
                    Credential = credential
                };

            var client =
                await
                    clientBuilder
                        .BuildAsync(cancellationToken);

            // Load the image and perform OCR
            var image = 
                Image
                    .FromBytes(imageBytes);

            var response =
                await
                    client
                        .DetectDocumentTextAsync(image);

            return
                response
                    .Text;
        }
    }
}