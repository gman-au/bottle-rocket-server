using System.Threading;
using System.Threading.Tasks;
using Rocket.Domain.Enum;

namespace Rocket.Ollama.Infrastructure
{
    public interface IOllamaClient
    {
        Task<T> SendRequestAsync<T>(
            string endpoint,
            string modelName,
            string prompt,
            byte[] imageBytes,
            RocketbookPageTemplateTypeEnum pageTemplateType,
            CancellationToken cancellationToken
        ) where T : class;
    }
}