using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Rocket.Api.Contracts;
using Rocket.Api.Contracts.Connectors;
using Rocket.Interfaces;
using Rocket.Web.Infrastructure.Client.Extensions;

namespace Rocket.Web.Infrastructure.Client
{
    internal class ApiRequestConnectors(
        ILogger<ApiRequestConnectors> logger,
        IJsonResolverInstanceProvider jsonResolverInstanceProvider,
        IAuthenticatedApiClient authenticatedApiClient
    ) : ApiRequestBase(
        logger,
        jsonResolverInstanceProvider,
        authenticatedApiClient
    ), IApiRequestConnectors
    {
        public async Task<CreateConnectorResponse<T>> CreateConnectorAsync<T>(
            CreateConnectorRequest<T> request,
            CancellationToken cancellationToken
        ) where T : ConnectorSummary
        {
            Logger
                .LogInformation("Received Create Connector request");

            var response =
                await
                    AuthenticatedApiClient
                        .PostAsJsonAsync(
                            "/api/connectors/create",
                            request,
                            cancellationToken
                        );

            var result =
                await
                    response
                        .TryParseResponse<CreateConnectorResponse<T>>(
                            JsonSerializerOptions,
                            Logger,
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
            Logger
                .LogInformation("Received get connectors request");

            var response =
                await
                    AuthenticatedApiClient
                        .PostAsJsonAsync(
                            "/api/connectors/fetch",
                            request,
                            cancellationToken
                        );

            var result =
                await
                    response
                        .TryParseResponse<FetchConnectorsResponse>(
                            JsonSerializerOptions,
                            Logger,
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
            Logger
                .LogInformation("Received Delete Connector request");

            var response =
                await
                    AuthenticatedApiClient
                        .DeleteAsync(
                            $"/api/connectors/{id}",
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