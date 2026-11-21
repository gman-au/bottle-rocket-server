using System.Threading;
using System.Threading.Tasks;
using Rocket.Ollama.Contracts;

namespace Rocket.Web.Infrastructure.Client.Vendors
{
    public interface IApiRequestOllama
    {
        Task<OllamaListModelsResponse> GetOllamaModelsAsync(
            OllamaListModelsRequest request,
            CancellationToken cancellationToken
        );
    }
}