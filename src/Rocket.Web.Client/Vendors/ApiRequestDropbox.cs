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
        public async Task<ApiResponse> FinalizeDropboxConnectorAsync(
            FinalizeDropboxConnectorRequest request,
            CancellationToken cancellationToken
        )
        {
            LoggerExtensions.LogInformation(logger, "Received Patch (Dropbox) Connector request");

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
                    HttpResponseMessageEx
                        .TryParseResponse<ApiResponse>(
                            response, logger,
                            cancellationToken
                        );

            Client.ApiRequestManager.EnsureApiSuccessStatusCode(result);
            Client.ApiRequestManager.EnsureHttpSuccessStatusCode(response);

            return result;
        }
    }
}