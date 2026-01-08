using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Rocket.Api.Contracts;
using Rocket.Domain.Enum;
using Rocket.Domain.Exceptions;
using Rocket.Web.Host.Authentication;
using Rocket.Web.Host.Extensions;

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

        public async Task<UserDetail> GetUserByIdAsync(
            string id,
            CancellationToken cancellationToken)
        {
            logger
                .LogInformation("Received Get User request");

            var response =
                await
                    authenticatedApiClient
                        .GetAsync(
                            $"/api/users/{id}",
                            cancellationToken
                        );

            var result =
                await
                    response
                        .TryParseResponse<UserDetail>(
                            logger,
                            cancellationToken
                        );

            EnsureApiSuccessStatusCode(result);
            EnsureHttpSuccessStatusCode(response);

            return result;
        }

        public async Task<UpdateUserResponse> UpdateUserAsync(
            UserDetail user,
            CancellationToken cancellationToken)
        {
            logger
                .LogInformation("Received Update User request");

            var response =
                await
                    authenticatedApiClient
                        .PostAsJsonAsync(
                            "/api/users/update",
                            user,
                            cancellationToken
                        );

            var result =
                await
                    response
                        .TryParseResponse<UpdateUserResponse>(
                            logger,
                            cancellationToken
                        );

            EnsureApiSuccessStatusCode(result);
            EnsureHttpSuccessStatusCode(response);

            return result;
        }

        public async Task<CreateUserResponse> CreateUserAsync(
            CreateUserRequest user, 
            CancellationToken cancellationToken
            )
        {
            logger
                .LogInformation("Received Create User request");

            var response =
                await
                    authenticatedApiClient
                        .PostAsJsonAsync(
                            "/api/users/create",
                            user,
                            cancellationToken
                        );

            var result =
                await
                    response
                        .TryParseResponse<CreateUserResponse>(
                            logger,
                            cancellationToken
                        );

            EnsureApiSuccessStatusCode(result);
            EnsureHttpSuccessStatusCode(response);

            return result;
        }

        public async Task<StartupPhaseResponse> GetStartupPhaseAsync(CancellationToken cancellationToken)
        {
            logger
                .LogInformation("Checking startup phase");

            var response =
                await
                    authenticatedApiClient
                        .GetAsync(
                            "/api/startup/phase",
                            cancellationToken
                        );

            response
                .EnsureSuccessStatusCode();

            var result =
                await
                    response
                        .TryParseResponse<StartupPhaseResponse>(
                            logger,
                            cancellationToken
                        );

            return result;
        }

        private static void EnsureApiSuccessStatusCode(ApiResponse response)
        {
            if (response == null) return;

            if (response.ErrorCode != (int)ApiStatusCodeEnum.Ok)
                throw new RocketException(
                    response.ErrorMessage,
                    response.ErrorCode
                );
        }

        private static void EnsureHttpSuccessStatusCode(HttpResponseMessage response)
        {
            try
            {
                response
                    .EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                var statusCode = (int)response.StatusCode;

                if (ex.StatusCode != null)
                    throw new RocketException(
                        "The server returned a bad response",
                        ApiStatusCodeEnum.ServerError,
                        statusCode
                    );
            }
        }
    }
}