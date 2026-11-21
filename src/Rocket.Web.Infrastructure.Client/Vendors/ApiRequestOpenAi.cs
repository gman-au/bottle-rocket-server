using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Rocket.Interfaces;
using Rocket.OpenAi.Contracts;
using Rocket.Web.Infrastructure.Client.Extensions;

namespace Rocket.Web.Infrastructure.Client.Vendors
{
    internal class ApiRequestOpenAi(
        ILogger<ApiRequestOpenAi> logger,
        IJsonResolverInstanceProvider jsonResolverInstanceProvider,
        IAuthenticatedApiClient authenticatedApiClient
    ) : ApiRequestBase(
        logger,
        jsonResolverInstanceProvider,
        authenticatedApiClient
    ), IApiRequestOpenAi
    {
        public async Task<OpenAiListModelsResponse> GetOpenAiModelsAsync(
            OpenAiListModelsRequest request,
            CancellationToken cancellationToken
        )
        {
            Logger
                .LogInformation("Received Get (OpenAI) models request");

            var response =
                await
                    AuthenticatedApiClient
                        .PostAsJsonAsync(
                            "/api/openai/models",
                            request,
                            cancellationToken
                        );

            var result =
                await
                    response
                        .TryParseResponse<OpenAiListModelsResponse>(
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