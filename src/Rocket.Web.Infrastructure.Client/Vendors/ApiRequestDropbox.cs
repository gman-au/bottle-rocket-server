using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Rocket.Api.Contracts;
using Rocket.Dropbox.Contracts;
using Rocket.Interfaces;
using Rocket.Web.Infrastructure.Client.Extensions;

namespace Rocket.Web.Infrastructure.Client.Vendors
{
    internal class ApiRequestDropbox(
        ILogger<ApiRequestDropbox> logger,
        IJsonResolverInstanceProvider jsonResolverInstanceProvider,
        IAuthenticatedApiClient authenticatedApiClient
    ) : ApiRequestBase(
        logger,
        jsonResolverInstanceProvider,
        authenticatedApiClient
    ), IApiRequestDropbox
    {
        public async Task<ApiResponse> FinalizeDropboxConnectorAsync(
            FinalizeDropboxConnectorRequest request,
            CancellationToken cancellationToken
        )
        {
            Logger
                .LogInformation("Received Patch (Dropbox) Connector request");

            var response =
                await
                    AuthenticatedApiClient
                        .PatchAsJsonAsync(
                            "/api/dropbox/connectors/finalize",
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
    }
}