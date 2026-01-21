using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Rocket.Api.Contracts;
using Rocket.Dropbox.Contracts;
using Rocket.Web.Client.Extensions;

namespace Rocket.Web.Client
{
    public partial class ApiRequestManager
    {
        public async Task<CreateDropboxConnectorResponse> CreateDropboxConnectorAsync(
            CreateDropboxConnectorRequest connector,
            CancellationToken cancellationToken
        )
        {
            logger.LogInformation("Received Create (Dropbox) Connector request");

            var response =
                await
                    authenticatedApiClient
                        .PostAsJsonAsync(
                            "/api/dropbox/connectors/create",
                            connector,
                            cancellationToken
                        );

            var result =
                await
                    response
                        .TryParseResponse<CreateDropboxConnectorResponse>(
                            logger,
                            cancellationToken
                        );

            EnsureApiSuccessStatusCode(result);
            EnsureHttpSuccessStatusCode(response);

            return result;
        }

        public async Task<ApiResponse> FinalizeDropboxConnectorAsync(
            FinalizeDropboxConnectorRequest request,
            CancellationToken cancellationToken
        )
        {
            logger.LogInformation("Received Patch (Dropbox) Connector request");

            var response =
                await
                    authenticatedApiClient
                        .PatchAsJsonAsync(
                            "/api/dropbox/connectors/finalize",
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