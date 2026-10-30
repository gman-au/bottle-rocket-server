using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Rocket.Api.Contracts;
using Rocket.Google.Contracts;
using Rocket.Interfaces;
using Rocket.Web.Infrastructure.Client.Extensions;

namespace Rocket.Web.Infrastructure.Client.Vendors
{
    internal class ApiRequestGoogle(
        ILogger<ApiRequestGoogle> logger,
        IJsonResolverInstanceProvider jsonResolverInstanceProvider,
        IAuthenticatedApiClient authenticatedApiClient
    ) : ApiRequestBase(
        logger,
        jsonResolverInstanceProvider,
        authenticatedApiClient
    ), IApiRequestGoogle
    {
        public async Task<GoogleAuthInitiateResponse> InitiateGoogleConnectorAuthAsync(
            GoogleAuthInitiateRequest request,
            CancellationToken cancellationToken
        )
        {
            Logger
                .LogInformation("Received Initiate (Google) Connector request");

            var response =
                await
                    AuthenticatedApiClient
                        .PostAsJsonAsync(
                            "/api/google/connectors/initiate",
                            request,
                            cancellationToken
                        );

            var result =
                await
                    response
                        .TryParseResponse<GoogleAuthInitiateResponse>(
                            JsonSerializerOptions,
                            Logger,
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
            Logger
                .LogInformation("Received Initiate (Google) Connector request");

            var response =
                await
                    AuthenticatedApiClient
                        .PatchAsJsonAsync(
                            "/api/google/connectors/finalize",
                            request,
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
        
        public async Task<GetGoogleDriveFoldersResponse> GetGoogleFoldersAsync(
            GetGoogleDriveFoldersRequest request,
            CancellationToken cancellationToken
        )
        {
            Logger
                .LogInformation("Received Get (Google Drive) folders request");

            var response =
                await
                    AuthenticatedApiClient
                        .PostAsJsonAsync(
                            "/api/google/workflows/getFolders",
                            request,
                            cancellationToken
                        );

            var result =
                await
                    response
                        .TryParseResponse<GetGoogleDriveFoldersResponse>(
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