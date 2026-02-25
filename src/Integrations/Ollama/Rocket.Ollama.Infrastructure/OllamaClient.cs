using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
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
        ISchemaGenerator schemaGenerator,
        ISchemaDictionary schemaDictionary,
        IImageBase64Converter imageBase64Converter
    ) : IOllamaClient
    {
        private const string UserRole = "user";

        private static readonly JsonSerializerOptions DefaultJsonSerializationOptions = new()
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        public async Task<T>
            SendRequestAsync<T>(
                string endpoint,
                string modelName,
                string prompt,
                byte[] imageBytes,
                RocketbookPageTemplateTypeEnum pageTemplateType,
                bool useSchema,
                float? temperature,
                int? maxTokens,
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
                    Stream = false,
                    Temperature = temperature,
                    NumPredict = maxTokens
                };

            if (useSchema)
            {
                var pageTemplateSchemaType =
                    schemaDictionary
                        .From(pageTemplateType);

                var schemaString =
                    schemaGenerator
                        .Generate(pageTemplateSchemaType);

                request.Format =
                    JsonDocument
                        .Parse(schemaString).RootElement;
            }

            HttpResponseMessage response = null;
            try
            {
                response =
                    await
                        httpClient
                            .PostAsJsonAsync(
                                "api/chat",
                                request,
                                DefaultJsonSerializationOptions,
                                cancellationToken
                            );

                response
                    .EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                throw new RocketException(
                    $"There was an error sending the request to the Ollama API: {ex.Message}",
                    ApiStatusCodeEnum.ThirdPartyServiceError
                );
            }

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