using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Rocket.Api.Contracts.Dashboard;
using Rocket.Interfaces;
using Rocket.Web.Infrastructure.Client.Extensions;

namespace Rocket.Web.Infrastructure.Client
{
    internal class ApiRequestDashboard(
        ILogger<ApiRequestDashboard> logger,
        IJsonResolverInstanceProvider jsonResolverInstanceProvider,
        IAuthenticatedApiClient authenticatedApiClient
    ) : ApiRequestBase(
        logger,
        jsonResolverInstanceProvider,
        authenticatedApiClient
    ), IApiRequestDashboard
    {
        public async Task<FetchDashboardResponse> GetDashboardSnapshotAsync(
            CancellationToken cancellationToken
        )
        {
            Logger
                .LogInformation("Received Dashboard request");

            var response =
                await
                    AuthenticatedApiClient
                        .GetAsync(
                            $"/api/dashboard",
                            cancellationToken
                        );

            var result =
                await
                    response
                        .TryParseResponse<FetchDashboardResponse>(
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