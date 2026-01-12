using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Rocket.Api.Contracts;
using Rocket.Web.Host.Extensions;

namespace Rocket.Web.Host.Api
{
    public partial class ApiRequestManager
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

            var result =
                await
                    response
                        .TryParseResponse<MyScansResponse>(
                            logger,
                            cancellationToken
                        );

            EnsureApiSuccessStatusCode(result);
            EnsureHttpSuccessStatusCode(response);

            return result;
        }

        public async Task<MyScanItemDetail> GetMyScanAsync(
            string id,
            CancellationToken cancellationToken)
        {
            logger
                .LogInformation("Received My Scan request");

            var response =
                await
                    authenticatedApiClient
                        .GetAsync(
                            $"/api/scans/{id}",
                            cancellationToken
                        );

            var result =
                await
                    response
                        .TryParseResponse<MyScanItemDetail>(
                            logger,
                            cancellationToken
                        );

            EnsureApiSuccessStatusCode(result);
            EnsureHttpSuccessStatusCode(response);

            return result;
        }
    }
}