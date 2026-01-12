using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Rocket.Api.Contracts;
using Rocket.Api.Contracts.Connectors;
using Rocket.Web.Host.Extensions;

namespace Rocket.Web.Host.Api
{
    public partial class ApiRequestManager
    {
        public async Task<DropboxConnectorDetail> GetDropboxConnectorByIdAsync(
            string id,
            CancellationToken cancellationToken
        )
        {
            logger
                .LogInformation("Received Get (Dropbox) Connector request");

            var response =
                await
                    authenticatedApiClient
                        .GetAsync(
                            $"/api/connectors/{id}",
                            cancellationToken
                        );

            var result =
                await
                    response
                        .TryParseResponse<DropboxConnectorDetail>(
                            logger,
                            cancellationToken
                        );

            EnsureApiSuccessStatusCode(result);
            EnsureHttpSuccessStatusCode(response);

            return result;
        }
        
        public async Task<ApiResponse> DeleteDropboxConnectorByIdAsync(
            string id,
            CancellationToken cancellationToken
        )
        {
            logger
                .LogInformation("Received Delete (Dropbox) Connector request");

            var response =
                await
                    authenticatedApiClient
                        .DeleteAsync(
                            $"/api/connectors/dropbox/{id}",
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

        public async Task<UpdateConnectorResponse> UpdateDropboxConnectorAsync(
            DropboxConnectorDetail connector,
            CancellationToken cancellationToken
        )
        {
            logger
                .LogInformation("Received Patch (Dropbox) Connector request");

            var response =
                await
                    authenticatedApiClient
                        .PatchAsJsonAsync(
                            $"/api/connectors/dropbox/update",
                            connector,
                            cancellationToken
                        );

            var result =
                await
                    response
                        .TryParseResponse<UpdateConnectorResponse>(
                            logger,
                            cancellationToken
                        );

            EnsureApiSuccessStatusCode(result);
            EnsureHttpSuccessStatusCode(response);

            return result;
        }
    }
}