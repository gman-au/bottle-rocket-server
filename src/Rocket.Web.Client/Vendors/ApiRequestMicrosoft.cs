using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Rocket.Microsofts.Contracts;
using Rocket.Web.Client.Extensions;

namespace Rocket.Web.Client
{
    public partial class ApiRequestManager
    {
        public async Task<MicrosoftAuthInitiateResponse> InitiateMicrosoftConnectorAuthAsync(
            MicrosoftAuthInitiateRequest request,
            CancellationToken cancellationToken
        )
        {
            logger
                .LogInformation("Received Initiate (Microsoft) Connector request");

            var response =
                await
                    authenticatedApiClient
                        .PostAsJsonAsync(
                            "/api/microsoft/connectors/initiate",
                            request,
                            cancellationToken
                        );

            var result =
                await
                    response
                        .TryParseResponse<MicrosoftAuthInitiateResponse>(
                            logger,
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
            logger
                .LogInformation("Received Get (OneNote) sections request");

            var response =
                await
                    authenticatedApiClient
                        .PostAsJsonAsync(
                            "/api/microsoft/workflows/getOneNoteSections",
                            request,
                            cancellationToken
                        );

            var result =
                await
                    response
                        .TryParseResponse<GetOneNoteSectionsResponse>(
                            logger,
                            cancellationToken
                        );

            EnsureApiSuccessStatusCode(result);
            EnsureHttpSuccessStatusCode(response);

            return result;
        }
    }
}