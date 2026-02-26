using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
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
        IImageBase64Converter imageBase64Converter,
        ILogger<OllamaClient> logger
    ) : IOllamaClient
    {
        private const string UserRole = "user";

        private static readonly JsonSerializerOptions DefaultJsonSerializationOptions = new()
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        public async Task<IEnumerable<string>> GetModelListAsync(string endpoint, CancellationToken cancellationToken)
        {
            using var httpClient = new HttpClient();

            httpClient.BaseAddress = new Uri(endpoint);
            
            var response =
                await
                    httpClient
                        .GetAsync(
                            "api/tags",
                            cancellationToken
                        );

            response
                .EnsureSuccessStatusCode();

            var tagsListResponse =
                await
                    response
                        .Content
                        .ReadFromJsonAsync<OllamaTagsResponse>(cancellationToken);

            var results =
                (tagsListResponse
                    .Models ?? [])
                .Select(o => o.Model);

            return results;
        }

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
                int? numCtx,
                CancellationToken cancellationToken
            ) where T : class
        {
            string base64Image = null;

            if (imageBytes != null)
            {
                base64Image =
                    imageBase64Converter
                        .Perform(imageBytes);
            }

            using var httpClient = new HttpClient();

            httpClient.BaseAddress = new Uri(endpoint);

            httpClient.Timeout =
                TimeSpan
                    .FromMinutes(10);

            logger
                .LogDebug("Ollama prompt:\r\n{prompt}", prompt);
            
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
                            Images = string.IsNullOrEmpty(base64Image) ? null : [base64Image]
                        }
                    ],
                    Stream = false,
                    Temperature = temperature,
                    NumPredict = maxTokens,
                    NumCtx = numCtx
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

            var response =
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

            var ollamaResponse =
                await
                    response
                        .Content
                        .ReadFromJsonAsync<OllamaOcrResponse>(cancellationToken);

            var messageContent =
                ollamaResponse?
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