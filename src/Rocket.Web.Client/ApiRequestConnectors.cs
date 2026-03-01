using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Rocket.Api.Contracts;
using Rocket.Api.Contracts.Connectors;
using Rocket.Web.Client.Extensions;

namespace Rocket.Web.Client
{
    public partial class ApiRequestManager
    {
        public async Task<CreateConnectorResponse<T>> CreateConnectorAsync<T>(
            CreateConnectorRequest<T> request,
            CancellationToken cancellationToken
        ) where T : ConnectorSummary
        {
            logger.LogInformation("Received Create Connector request");

            var response =
                await
                    authenticatedApiClient
                        .PostAsJsonAsync(
                            "/api/connectors/create",
                            request,
                            cancellationToken
                        );

            var result =
                await
                    response
                        .TryParseResponse<CreateConnectorResponse<T>>(
                            _jsonSerializerOptions,
                            logger,
                            cancellationToken
                        );

            EnsureApiSuccessStatusCode(result);
            EnsureHttpSuccessStatusCode(response);

            return result;
        }
        
        public async Task<FetchConnectorsResponse> GetMyConnectorsAsync(
            FetchConnectorsRequest request, 
            CancellationToken cancellationToken
        )
        {
            logger
                .LogInformation("Received get connectors request");

            var response =
                await
                    authenticatedApiClient
                        .PostAsJsonAsync(
                            "/api/connectors/fetch",
                            request,
                            cancellationToken
                        );

            var result =
                await
                    response
                        .TryParseResponse<FetchConnectorsResponse>(
                            _jsonSerializerOptions,
                            logger,
                            cancellationToken
                        );

            EnsureApiSuccessStatusCode(result);
            EnsureHttpSuccessStatusCode(response);

            return result;
        }
        
        public async Task<ApiResponse> DeleteConnectorByIdAsync(
            string id,
            CancellationToken cancellationToken
        )
        {
            logger
                .LogInformation("Received Delete Connector request");

            var response =
                await
                    authenticatedApiClient
                        .DeleteAsync(
                            $"/api/connectors/{id}",
                            cancellationToken
                        );

            var result =
                await
                    response
                        .TryParseResponse<ApiResponse>(
                            _jsonSerializerOptions,
                            logger,
                            cancellationToken
                        );

            EnsureApiSuccessStatusCode(result);
            EnsureHttpSuccessStatusCode(response);

            return result;
        }
    }
}