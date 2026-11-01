using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Rocket.Api.Contracts.Sessions;
using Rocket.Interfaces;
using Rocket.Web.Infrastructure.Client.Extensions;

namespace Rocket.Web.Infrastructure.Client
{
    internal class ApiRequestSessions(
        ILogger<ApiRequestSessions> logger,
        IJsonResolverInstanceProvider jsonResolverInstanceProvider,
        IAuthenticatedApiClient authenticatedApiClient
    ) : ApiRequestBase(
        logger,
        jsonResolverInstanceProvider,
        authenticatedApiClient
    ), IApiRequestSessions
    {
        public async Task<MySessionsResponse> GetMySessionsAsync(
            MySessionsRequest request,
            CancellationToken cancellationToken
        )
        {
            Logger
                .LogInformation("Received My Sessions request");

            var response =
                await
                    AuthenticatedApiClient
                        .PostAsJsonAsync(
                            "/api/sessions/fetch",
                            request,
                            cancellationToken
                        );

            var result =
                await
                    response
                        .TryParseResponse<MySessionsResponse>(
                            JsonSerializerOptions,
                            Logger,
                            cancellationToken
                        );

            EnsureApiSuccessStatusCode(result);
            EnsureHttpSuccessStatusCode(response);

            return result;
        }

        public async Task<RevokeSessionResponse> RevokeSessionAsync(
            string id,
            CancellationToken cancellationToken
        )
        {
            Logger
                .LogInformation("Received Revoke Session request");

            var response =
                await
                    AuthenticatedApiClient
                        .DeleteAsync(
                            $"/api/sessions/revoke/{id}",
                            cancellationToken
                        );

            var result =
                await
                    response
                        .TryParseResponse<RevokeSessionResponse>(
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