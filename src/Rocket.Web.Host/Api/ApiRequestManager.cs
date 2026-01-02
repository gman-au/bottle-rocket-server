using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Rocket.Api.Contracts;
using Rocket.Web.Host.Authentication;

namespace Rocket.Web.Host.Api
{
    public class ApiRequestManager(
        ILogger<ApiRequestManager> logger,
        IAuthenticatedApiClient authenticatedApiClient
    ) : IApiRequestManager
    {
        public async Task<MyScansResponse> GetMyScansAsync(
            MyScansRequest request,
            CancellationToken cancellationToken
        )
        {
            logger
                .LogInformation("Received My Scans request");

            var response =
                await
                    authenticatedApiClient
                        .PostAsJsonAsync(
                            "/api/scans/fetch",
                            request,
                            cancellationToken
                        );

            response
                .EnsureSuccessStatusCode();

            var result =
                await
                    response
                        .Content
                        .ReadFromJsonAsync<MyScansResponse>(cancellationToken: cancellationToken);

            return result;
        }
    }
}