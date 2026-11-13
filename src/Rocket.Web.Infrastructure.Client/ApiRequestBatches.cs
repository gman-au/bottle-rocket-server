using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Rocket.Api.Contracts;
using Rocket.Api.Contracts.Batches;
using Rocket.Interfaces;
using Rocket.Web.Infrastructure.Client.Extensions;

namespace Rocket.Web.Infrastructure.Client
{
    internal class ApiRequestBatches(
        ILogger<ApiRequestBatches> logger,
        IJsonResolverInstanceProvider jsonResolverInstanceProvider,
        IAuthenticatedApiClient authenticatedApiClient
    ) : ApiRequestBase(
        logger,
        jsonResolverInstanceProvider,
        authenticatedApiClient
    ), IApiRequestBatches
    {
        public async Task<MyBatchesResponse> GetMyBatchesAsync(
            MyBatchesRequest request,
            CancellationToken cancellationToken
        )
        {
            Logger
                .LogInformation("Received My Batches request");

            var response =
                await
                    AuthenticatedApiClient
                        .PostAsJsonAsync(
                            "/api/batches/fetch",
                            request,
                            cancellationToken
                        );

            var result =
                await
                    response
                        .TryParseResponse<MyBatchesResponse>(
                            JsonSerializerOptions,
                            Logger,
                            cancellationToken
                        );

            EnsureApiSuccessStatusCode(result);
            EnsureHttpSuccessStatusCode(response);

            return result;
        }

        public async Task<BatchSpecifics> GetMyBatchAsync(
            string id,
            CancellationToken cancellationToken
        )
        {
            Logger
                .LogInformation("Received My Batch request");

            var response =
                await
                    AuthenticatedApiClient
                        .GetAsync(
                            $"/api/batches/{id}",
                            cancellationToken
                        );

            var result =
                await
                    response
                        .TryParseResponse<BatchSpecifics>(
                            JsonSerializerOptions,
                            Logger,
                            cancellationToken
                        );

            EnsureApiSuccessStatusCode(result);
            EnsureHttpSuccessStatusCode(response);

            return result;
        }

        public async Task<ApiResponse> DeleteBatchByIdAsync(
            string id,
            CancellationToken cancellationToken
        )
        {
            Logger
                .LogInformation("Received Delete Batch request");

            var response =
                await
                    AuthenticatedApiClient
                        .DeleteAsync(
                            $"/api/batches/delete/{id}",
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