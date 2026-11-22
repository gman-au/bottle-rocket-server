using System.Threading;
using System.Threading.Tasks;
using Rocket.OpenAi.Contracts;

namespace Rocket.Web.Infrastructure.Client.Vendors
{
    public interface IApiRequestOpenAi
    {
        Task<OpenAiListModelsResponse> GetOpenAiModelsAsync(
            OpenAiListModelsRequest request,
            CancellationToken cancellationToken
        );
    }
}