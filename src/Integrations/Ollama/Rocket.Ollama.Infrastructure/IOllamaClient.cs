using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Rocket.Domain.Enum;

namespace Rocket.Ollama.Infrastructure
{
    public interface IOllamaClient
    {
        Task<IEnumerable<string>> GetModelListAsync(
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