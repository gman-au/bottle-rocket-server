using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Rocket.Api.Contracts;
using Rocket.Google.Contracts;
using Rocket.Web.Client.Extensions;

namespace Rocket.Web.Client
{
    public partial class ApiRequestManager
    {
        public async Task<GoogleAuthInitiateResponse> InitiateGoogleConnectorAuthAsync(
            GoogleAuthInitiateRequest request,
            CancellationToken cancellationToken
        )
        {
            logger
                .LogInformation("Received Initiate (Google) Connector request");

            var response =
                await
                    authenticatedApiClient
                        .PostAsJsonAsync(
                            "/api/google/connectors/initiate",
                            request,
                            cancellationToken
                        );

            var result =
                await
                    response
                        .TryParseResponse<GoogleAuthInitiateResponse>(
                            logger,
                            cancellationToken
                        );

            EnsureApiSuccessStatusCode(result);
            EnsureHttpSuccessStatusCode(response);

            return result;
        }

        public async Task<ApiResponse> FinalizeGoogleConnectorAsync(
            GoogleAuthFinalizeRequest request,
            CancellationToken cancellationToken
        )
        {
            logger
                .LogInformation("Received Initiate (Google) Connector request");

            var response =
                await
                    authenticatedApiClient
                        .PatchAsJsonAsync(
                            "/api/google/connectors/finalize",
                            request,
                            cancellationToken
                        );

            var result =
                await
                    response
                        .TryParseResponse<ApiResponse>(
                            logger,
                            cancellationToken
                        );

            EnsureApiSuccessStatusCode(result);
            EnsureHttpSuccessStatusCode(response);

            return result;
        }
    }
}