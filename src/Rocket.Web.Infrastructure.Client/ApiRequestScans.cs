using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Rocket.Api.Contracts;
using Rocket.Api.Contracts.Scans;
using Rocket.Interfaces;
using Rocket.Web.Infrastructure.Client.Extensions;

[assembly: InternalsVisibleTo("Rocket.Tests.Unit")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]

namespace Rocket.Web.Infrastructure.Client
{
    internal class ApiRequestScans(
        ILogger<ApiRequestScans> logger,
        IJsonResolverInstanceProvider jsonResolverInstanceProvider,
        IAuthenticatedApiClient authenticatedApiClient
    ) : ApiRequestBase(
        logger,
        jsonResolverInstanceProvider,
        authenticatedApiClient
    ), IApiRequestScans
    {
        public async Task<MyScansResponse> GetMyScansAsync(
            MyScansRequest request,
            CancellationToken cancellationToken
        )
        {
            Logger
                .LogInformation("Received My Scans request");

            var response =
                await
                    AuthenticatedApiClient
                        .PostAsJsonAsync(
                            "/api/scans/fetch",
                            request,
                            cancellationToken
                        );

            var result =
                await
                    response
                        .TryParseResponse<MyScansResponse>(
                            JsonSerializerOptions,
                            Logger,
                            cancellationToken
                        );

            EnsureApiSuccessStatusCode(result);
            EnsureHttpSuccessStatusCode(response);

            return result;
        }

        public async Task<ScanSpecifics> GetMyScanAsync(
            string id,
            CancellationToken cancellationToken
        )
        {
            Logger
                .LogInformation("Received My Scan request");

            var response =
                await
                    AuthenticatedApiClient
                        .GetAsync(
                            $"/api/scans/{id}",
                            cancellationToken
                        );

            var result =
                await
                    response
                        .TryParseResponse<ScanSpecifics>(
                            JsonSerializerOptions,
                            Logger,
                            cancellationToken
                        );

            EnsureApiSuccessStatusCode(result);
            EnsureHttpSuccessStatusCode(response);

            return result;
        }

        public async Task<ApiResponse> DeleteScanByIdAsync(
            string id,
            CancellationToken cancellationToken
        )
        {
            Logger
                .LogInformation("Received Delete Scan request");

            var response =
                await
                    AuthenticatedApiClient
                        .DeleteAsync(
                            $"/api/scans/delete/{id}",
                            cancellationToken
                        );

            var result =
                await
                    response
                        .TryParseResponse<ApiResponse>(
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