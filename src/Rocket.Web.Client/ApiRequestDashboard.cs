using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Rocket.Api.Contracts.Dashboard;
using Rocket.Web.Client.Extensions;

namespace Rocket.Web.Client
{
    public partial class ApiRequestManager
    {
        public async Task<FetchDashboardResponse> GetDashboardSnapshotAsync(
            CancellationToken cancellationToken
        )
        {
            logger
                .LogInformation("Received Dashboard request");

            var response =
                await
                    authenticatedApiClient
                        .GetAsync(
                            $"/api/dashboard",
                            cancellationToken
                        );

            var result =
                await
                    response
                        .TryParseResponse<FetchDashboardResponse>(
                            _jsonSerializerOptions,
                            logger,
                            cancellationToken
                        );

            EnsureApiSuccessStatusCode(result);
            EnsureHttpSuccessStatusCode(response);

            return result;
        }
    }
}