using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Rocket.Ollama.Infrastructure.Definition;

namespace Rocket.Ollama.Infrastructure
{
    public interface IOllamaClient
    {
        Task<IEnumerable<OllamaTagsModel>> GetModelListAsync(
            string endpoint,
            CancellationToken cancellationToken
        );

        Task<T> SendImageRequestAsync<T>(
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
        ) where T : class;

        Task<T> SendTextRequestAsync<T>(
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
        ) where T : class;
    }
}