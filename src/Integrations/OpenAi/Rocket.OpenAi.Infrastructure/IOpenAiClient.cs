using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Rocket.OpenAi.Infrastructure.Definition;

namespace Rocket.OpenAi.Infrastructure
{
    public interface IOpenAiClient
    {
        Task<IEnumerable<OpenAiTagsModel>> GetModelListAsync(
            string apiKey,
            CancellationToken cancellationToken
        );

        Task<T> SendImageRequestAsync<T>(
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
        ) where T : class;

        Task<T> SendTextRequestAsync<T>(
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
        ) where T : class;
    }
}