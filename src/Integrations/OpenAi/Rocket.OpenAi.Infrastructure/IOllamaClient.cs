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
    }
}