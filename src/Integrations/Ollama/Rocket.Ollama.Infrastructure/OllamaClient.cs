using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Rocket.Domain.Enum;
using Rocket.Domain.Exceptions;
using Rocket.Interfaces;
using Rocket.Ollama.Infrastructure.Definition;

namespace Rocket.Ollama.Infrastructure
{
    public class OllamaClient(
        ISchemaResponseBuilder schemaResponseBuilder,
        IImageBase64Converter imageBase64Converter
    ) : IOllamaClient
    {
        private const string UserRole = "user";

        public async Task<T>
            SendRequestAsync<T>(
                string endpoint,
                string modelName,
                string prompt,
                byte[] imageBytes,
                RocketbookPageTemplateTypeEnum pageTemplateType,
                CancellationToken cancellationToken
            ) where T : class
        {
            var base64Image =
                imageBase64Converter
                    .Perform(imageBytes);

            using var httpClient = new HttpClient();

            httpClient.BaseAddress = new Uri(endpoint);

            httpClient.Timeout =
                TimeSpan
                    .FromMinutes(10);

            var request =
                new OllamaOcrRequest
                {
                    Model = modelName,
                    Messages =
                    [
                        new OllamaOcrRequestMessage
                        {
                            Role = UserRole,
                            Content = prompt,
                            Images = [base64Image]
                        }
                    ],
                    Stream = false
                };

            var response =
                await
                    httpClient
                        .PostAsJsonAsync(
                            "api/chat",
                            request,
                            cancellationToken
                        );

            response
                .EnsureSuccessStatusCode();

            var ocrResponse =
                await
                    response
                        .Content
                        .ReadFromJsonAsync<OllamaOcrResponse>(cancellationToken);

            var messageContent =
                ocrResponse?
                    .Message?
                    .Content;

            var data =
                schemaResponseBuilder
                    .Build(
                        messageContent,
                        pageTemplateType
                    );

            if (data is not T typedData)
                throw new RocketException(
                    $"There was an error parsing the data returned from the model to type {typeof(T)}.",
                    ApiStatusCodeEnum.ThirdPartyServiceError
                );

            return typedData;
        }
    }
}