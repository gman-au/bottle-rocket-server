using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Rocket.Domain.Enum;
using Rocket.Domain.Exceptions;
using Rocket.Replicate.Infrastructure.Definition;

namespace Rocket.Replicate.Infrastructure
{
    public class BaseReplicateClient
    {
        private const string ReplicateEndpoint = "https://api.replicate.com/";

        protected HttpClient GetBaseHttpClient(string apiToken)
        {
            var httpClient = new HttpClient();

            httpClient.BaseAddress =
                new Uri(ReplicateEndpoint);

            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(
                    "Bearer",
                    apiToken
                );

            return httpClient;
        }

        protected async Task<Exception> HandleReplicateExceptionAsync(
            HttpResponseMessage responseMessage,
            CancellationToken cancellationToken
        )
        {
            var parsedError =
                await
                    responseMessage?
                        .Content?
                        .ReadFromJsonAsync<ReplicateErrorResponse>(cancellationToken);

            throw new RocketException(
                parsedError?.Detail ?? "There was an error processing the image",
                ApiStatusCodeEnum.ThirdPartyServiceError
            );
        }
    }
}