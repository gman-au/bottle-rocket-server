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
        IImageBase64Converter imageBase64Converter,
        ILogger<OllamaClient> logger
    ) : IOllamaClient
    {
        private const string SystemRole = "system";
        private const string UserRole = "user";

        private static readonly JsonSerializerOptions DefaultJsonSerializationOptions = new()
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        public async Task<IEnumerable<string>> GetModelListAsync(
            string endpoint,
            CancellationToken cancellationToken
        )
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

        public async Task<T> SendImageRequestAsync<T>(
            string endpoint,
            string modelName,
            string prompt,
            byte[] imageBytes,
            Type schemaType,
            bool useSchema,
            float? temperature,
            int? maxTokens,
            int? numCtx,
            int timeoutInMinutes,
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

            var promptMessages = new[]
            {
                new OllamaOcrRequestMessage
                {
                    Role = UserRole,
                    Content = prompt,
                    Images = string.IsNullOrEmpty(base64Image) ? null : [base64Image]
                }
            };

            return
                await
                    SendRequestAsync<T>(
                        endpoint,
                        modelName,
                        prompt,
                        schemaType,
                        useSchema,
                        temperature,
                        maxTokens,
                        numCtx,
                        timeoutInMinutes,
                        cancellationToken,
                        promptMessages
                    );
        }
        
        public async Task<T> SendTextRequestAsync<T>(
            string endpoint,
            string modelName,
            string systemPrompt,
            string userPrompt,
            Type schemaType,
            bool useSchema,
            float? temperature,
            int? maxTokens,
            int? numCtx,
            int timeoutInMinutes,
            CancellationToken cancellationToken
        ) where T : class
        {
            var promptMessages = new[]
            {
                new OllamaOcrRequestMessage
                {
                    Role = SystemRole,
                    Content = systemPrompt
                },
                new OllamaOcrRequestMessage
                {
                    Role = UserRole,
                    Content = userPrompt
                }
            };

            return
                await
                    SendRequestAsync<T>(
                        endpoint,
                        modelName,
                        systemPrompt,
                        schemaType,
                        useSchema,
                        temperature,
                        maxTokens,
                        numCtx,
                        timeoutInMinutes,
                        cancellationToken,
                        promptMessages
                    );
        }

        private async Task<T> SendRequestAsync<T>(
            string endpoint,
            string modelName,
            string prompt,
            Type schemaType,
            bool useSchema,
            float? temperature,
            int? maxTokens,
            int? numCtx,
            int timeoutInMinutes,
            CancellationToken cancellationToken,
            params OllamaOcrRequestMessage[] promptMessages
        ) where T : class
        {
            using var httpClient = new HttpClient();

            httpClient.BaseAddress = new Uri(endpoint);

            httpClient.Timeout =
                TimeSpan
                    .FromMinutes(timeoutInMinutes);

            logger
                .LogDebug(
                    "Ollama prompt:\r\n{prompt}",
                    prompt
                );

            var request =
                new OllamaOcrRequest
                {
                    Model = modelName,
                    Messages = promptMessages,
                    Stream = false,
                    Temperature = temperature,
                    NumPredict = maxTokens,
                    NumCtx = numCtx
                };

            if (useSchema)
            {
                var schemaString =
                    schemaGenerator
                        .Generate(schemaType);

                request.Format =
                    JsonDocument
                        .Parse(schemaString)
                        .RootElement;
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
                        schemaType
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