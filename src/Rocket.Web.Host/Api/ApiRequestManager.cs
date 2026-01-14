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
    public partial class ApiRequestManager(
        ILogger<ApiRequestManager> logger,
        IAuthenticatedApiClient authenticatedApiClient
    ) : IApiRequestManager
    {
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