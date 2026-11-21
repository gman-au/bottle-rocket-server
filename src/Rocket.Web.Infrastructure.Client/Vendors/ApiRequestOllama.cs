using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Rocket.Interfaces;
using Rocket.Ollama.Contracts;
using Rocket.Web.Infrastructure.Client.Extensions;

namespace Rocket.Web.Infrastructure.Client.Vendors
{
    internal class ApiRequestOllama(
        ILogger<ApiRequestOllama> logger,
        IJsonResolverInstanceProvider jsonResolverInstanceProvider,
        IAuthenticatedApiClient authenticatedApiClient
    ) : ApiRequestBase(
        logger,
        jsonResolverInstanceProvider,
        authenticatedApiClient
    ), IApiRequestOllama
    {
        public async Task<OllamaListModelsResponse> GetOllamaModelsAsync(
            OllamaListModelsRequest request,
            CancellationToken cancellationToken
        )
        {
            Logger
                .LogInformation("Received Get (Ollama) models request");

            var response =
                await
                    AuthenticatedApiClient
                        .PostAsJsonAsync(
                            "/api/ollama/models",
                            request,
                            cancellationToken
                        );

            var result =
                await
                    response
                        .TryParseResponse<OllamaListModelsResponse>(
                            JsonSerializerOptions,
                            Logger,
                            cancellationToken
                        );

            EnsureApiSuccessStatusCode(result);
            EnsureHttpSuccessStatusCode(response);

            return result;
        }
    }
}