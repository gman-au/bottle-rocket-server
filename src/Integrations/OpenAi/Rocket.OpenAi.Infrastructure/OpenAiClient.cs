using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using OpenAI;
using OpenAI.Chat;
using Rocket.Domain.Enum;
using Rocket.Domain.Exceptions;
using Rocket.Interfaces;
using Rocket.OpenAi.Infrastructure.Definition;

namespace Rocket.OpenAi.Infrastructure
{
    public class OpenAiClient(
        ISchemaResponseBuilder schemaResponseBuilder,
        ISchemaGenerator schemaGenerator,
        ILogger<OpenAiClient> logger
    ) : IOpenAiClient
    {
        public async Task<IEnumerable<OpenAiTagsModel>> GetModelListAsync(
            string apiKey,
            CancellationToken cancellationToken
        )
        {
            try
            {
                var openAi = new OpenAIClient(apiKey);

                var modelClient =
                    openAi
                        .GetOpenAIModelClient();

                var models =
                    await
                        modelClient
                            .GetModelsAsync(cancellationToken);

                var results =
                    models
                        .Value
                        .Select(o => new OpenAiTagsModel { Id = o.Id })
                        .OrderBy(o => o.Id);

                return results;
            }
            catch (Exception ex)
            {
                logger
                    .LogError(
                        "There was an error retrieving the OpenAI models: {message}",
                        ex.Message
                    );

                throw;
            }
        }

        public async Task<T> SendTextRequestAsync<T>(
            string apiKey,
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
            var client =
                new ChatClient(
                    modelName,
                    apiKey
                );

            var messages =
                new List<ChatMessage>
                {
                    new SystemChatMessage(
                        systemPrompt
                    ),

                    new UserChatMessage(
                        ChatMessageContentPart
                            .CreateTextPart(userPrompt)
                    )
                };

            var chatCompletionOptions =
                new ChatCompletionOptions
                {
                    Temperature = temperature,
                    MaxOutputTokenCount = maxTokens
                };

            if (useSchema)
            {
                var schemaString =
                    schemaGenerator
                        .Generate(schemaType);

                chatCompletionOptions.ResponseFormat =
                    ChatResponseFormat
                        .CreateJsonSchemaFormat(
                            jsonSchemaFormatName: "response_format",
                            jsonSchema: BinaryData.FromString(schemaString)
                        );
            }

            var result =
                await
                    client
                        .CompleteChatAsync(
                            messages,
                            chatCompletionOptions,
                            cancellationToken
                        );

            if (!(result?.Value?.Content ?? []).Any())
                throw new RocketException(
                    $"No data was returned from the model response.",
                    ApiStatusCodeEnum.ThirdPartyServiceError
                );

            var responseText =
                result
                    .Value
                    .Content[0]
                    .Text;

            var data =
                schemaResponseBuilder
                    .Build(
                        responseText,
                        schemaType
                    );

            if (data is not T typedData)
                throw new RocketException(
                    $"There was an error parsing the data returned from the model to type {typeof(T)}.",
                    ApiStatusCodeEnum.ThirdPartyServiceError
                );

            return typedData;
        }

        public async Task<T> SendImageRequestAsync<T>(
            string apiKey,
            string modelName,
            string systemPrompt,
            string userPrompt,
            byte[] imageBytes,
            Type schemaType,
            float? temperature,
            int? maxTokens,
            int timeoutInMinutes,
            string mediaType,
            CancellationToken cancellationToken
        ) where T : class
        {
            if (imageBytes == null)
                throw new RocketException(
                    "No image data provided",
                    ApiStatusCodeEnum.ValidationError
                );

            var client =
                new ChatClient(
                    modelName,
                    apiKey
                );

            var chatCompletionOptions =
                new ChatCompletionOptions
                {
                    Temperature = temperature,
                    MaxOutputTokenCount = maxTokens
                };

            mediaType = $"image/{mediaType}";

            var messages =
                new List<ChatMessage>
                {
                    new SystemChatMessage(
                        systemPrompt
                    ),

                    new UserChatMessage(
                        ChatMessageContentPart
                            .CreateTextPart(userPrompt),
                        ChatMessageContentPart
                            .CreateImagePart(
                                BinaryData
                                    .FromBytes(imageBytes),
                                mediaType
                            )
                    )
                };

            var result =
                await
                    client
                        .CompleteChatAsync(
                            messages,
                            chatCompletionOptions,
                            cancellationToken
                        );

            if (!(result?.Value?.Content ?? []).Any())
                throw new RocketException(
                    "No data was returned from the model response.",
                    ApiStatusCodeEnum.ThirdPartyServiceError
                );

            var responseText =
                result
                    .Value
                    .Content[0]
                    .Text;

            var data =
                schemaResponseBuilder
                    .Build(
                        responseText,
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