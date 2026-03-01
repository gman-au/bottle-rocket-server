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

        Task<T> SendRequestAsync<T>(
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
        ) where T : class;
    }
}