using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Rocket.Interfaces;
using Rocket.Microsofts.Contracts;
using Rocket.Web.Client.Extensions;

namespace Rocket.Web.Client.Vendors
{
    internal class ApiRequestMicrosoft(
        ILogger<ApiRequestMicrosoft> logger,
        IJsonResolverInstanceProvider jsonResolverInstanceProvider,
        IAuthenticatedApiClient authenticatedApiClient
    ) : ApiRequestBase(
        logger,
        jsonResolverInstanceProvider,
        authenticatedApiClient
    ), IApiRequestMicrosoft
    {
        public async Task<MicrosoftAuthInitiateResponse> InitiateMicrosoftConnectorAuthAsync(
            MicrosoftAuthInitiateRequest request,
            CancellationToken cancellationToken
        )
        {
            Logger
                .LogInformation("Received Initiate (Microsoft) Connector request");

            var response =
                await
                    AuthenticatedApiClient
                        .PostAsJsonAsync(
                            "/api/microsoft/connectors/initiate",
                            request,
                            cancellationToken
                        );

            var result =
                await
                    response
                        .TryParseResponse<MicrosoftAuthInitiateResponse>(
                            JsonSerializerOptions,
                            Logger,
                            cancellationToken
                        );

            EnsureApiSuccessStatusCode(result);
            EnsureHttpSuccessStatusCode(response);

            return result;
        }
        
        public async Task<GetOneNoteSectionsResponse> GetOneNoteSectionsAsync(
            GetOneNoteSectionsRequest request,
            CancellationToken cancellationToken
        )
        {
            Logger
                .LogInformation("Received Get (OneNote) sections request");

            var response =
                await
                    AuthenticatedApiClient
                        .PostAsJsonAsync(
                            "/api/microsoft/workflows/getOneNoteSections",
                            request,
                            cancellationToken
                        );

            var result =
                await
                    response
                        .TryParseResponse<GetOneNoteSectionsResponse>(
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