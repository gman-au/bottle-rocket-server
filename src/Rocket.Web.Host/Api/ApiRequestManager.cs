using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Rocket.Api.Contracts;
using Rocket.Domain.Enum;
using Rocket.Domain.Exceptions;
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

            var result =
                await
                    ParseJsonOrNullAsync<MyScansResponse>(
                        response,
                        cancellationToken
                    );
            
            EnsureApiSuccessStatusCode(result);
            EnsureHttpSuccessStatusCode(response);

            return result;
        }

        public async Task<MyScanItemDetail> GetMyScanAsync(string id, CancellationToken cancellationToken)
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
                    ParseJsonOrNullAsync<MyScanItemDetail>(
                        response,
                        cancellationToken
                    );

            EnsureApiSuccessStatusCode(result);
            EnsureHttpSuccessStatusCode(response);

            return result;
        }

        private static void EnsureApiSuccessStatusCode(ApiResponse response)
        {
            if (response == null) return;

            if (response.ErrorCode != (int)ApiStatusCodeEnum.Ok)
            {
                throw new RocketException(
                    response.ErrorMessage,
                    response.ErrorCode
                );
            }
        }

        private static void EnsureHttpSuccessStatusCode(HttpResponseMessage response)
        {
            response
                .EnsureSuccessStatusCode();
        }

        private static async Task<T> ParseJsonOrNullAsync<T>(
            HttpResponseMessage response,
            CancellationToken cancellationToken
        )
        {
            try
            {
                var result =
                    await
                        response
                            .Content
                            .ReadFromJsonAsync<T>(cancellationToken: cancellationToken);

                return result;
            }
            catch (JsonException)
            {
                return default;
            }
        }
    }
}