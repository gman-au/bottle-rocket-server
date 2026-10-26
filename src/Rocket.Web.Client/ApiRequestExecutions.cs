using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Rocket.Api.Contracts.Executions;
using Rocket.Interfaces;
using Rocket.Web.Client.Extensions;

namespace Rocket.Web.Client
{
    internal class ApiRequestExecutions(
        ILogger<ApiRequestExecutions> logger,
        IJsonResolverInstanceProvider jsonResolverInstanceProvider,
        IAuthenticatedApiClient authenticatedApiClient
    ) : ApiRequestBase(
        logger,
        jsonResolverInstanceProvider,
        authenticatedApiClient
    ), IApiRequestExecutions
    {
        public async Task<FetchExecutionsResponse> GetExecutionsAsync(
            FetchExecutionsRequest request,
            CancellationToken cancellationToken
        )
        {
            Logger
                .LogInformation("Received get executions request");

            var response =
                await
                    AuthenticatedApiClient
                        .PostAsJsonAsync(
                            "/api/executions/fetch",
                            request,
                            cancellationToken
                        );

            var result =
                await
                    response
                        .TryParseResponse<FetchExecutionsResponse>(
                            JsonSerializerOptions,
                            Logger,
                            cancellationToken
                        );

            EnsureApiSuccessStatusCode(result);
            EnsureHttpSuccessStatusCode(response);

            return result;
        }
        
        public async Task<ExecutionSummary> GetExecutionByIdAsync(
            string id,
            CancellationToken cancellationToken
        )
        {
            Logger
                .LogInformation("Received Get Execution request");

            var response =
                await
                    AuthenticatedApiClient
                        .GetAsync(
                            $"/api/executions/get/{id}",
                            cancellationToken
                        );

            var result =
                await
                    response
                        .TryParseResponse<ExecutionSummary>(
                            JsonSerializerOptions,
                            Logger,
                            cancellationToken
                        );

            EnsureApiSuccessStatusCode(result);
            EnsureHttpSuccessStatusCode(response);

            return result;
        }
        
        public async Task<CreateExecutionResponse> CreateExecutionAsync(
            CreateExecutionRequest workflow,
            CancellationToken cancellationToken
        )
        {
            Logger
                .LogInformation("Received Create Execution request");

            var response =
                await
                    AuthenticatedApiClient
                        .PostAsJsonAsync(
                            "/api/executions/create",
                            workflow,
                            cancellationToken
                        );

            var result =
                await
                    response
                        .TryParseResponse<CreateExecutionResponse>(
                            JsonSerializerOptions,
                            Logger,
                            cancellationToken
                        );

            EnsureApiSuccessStatusCode(result);
            EnsureHttpSuccessStatusCode(response);

            return result;
        }

        public async Task<StartExecutionResponse> StartExecutionAsync(
            string executionId,
            CancellationToken cancellationToken
        )
        {
            Logger
                .LogInformation("Received Start Execution request");

            var response =
                await
                    AuthenticatedApiClient
                        .PutAsync(
                            $"/api/executions/start/{executionId}",
                            null,
                            cancellationToken
                        );

            var result =
                await
                    response
                        .TryParseResponse<StartExecutionResponse>(
                            JsonSerializerOptions,
                            Logger,
                            cancellationToken
                        );

            EnsureApiSuccessStatusCode(result);
            EnsureHttpSuccessStatusCode(response);

            return result;
        }

        public async Task<CancelExecutionResponse> CancelExecutionAsync(
            string executionId,
            CancellationToken cancellationToken
        )
        {
            Logger
                .LogInformation("Received Cancel Execution request");

            var response =
                await
                    AuthenticatedApiClient
                        .PutAsync(
                            $"/api/executions/cancel/{executionId}",
                            null,
                            cancellationToken
                        );

            var result =
                await
                    response
                        .TryParseResponse<CancelExecutionResponse>(
                            JsonSerializerOptions,
                            Logger,
                            cancellationToken
                        );

            EnsureApiSuccessStatusCode(result);
            EnsureHttpSuccessStatusCode(response);

            return result;
        }
        
        public async Task<DeleteExecutionResponse> DeleteExecutionByIdAsync(
            string executionId,
            CancellationToken cancellationToken
        )
        {
            Logger
                .LogInformation("Received Delete Execution request");

            var response =
                await
                    AuthenticatedApiClient
                        .DeleteAsync(
                            $"/api/executions/{executionId}",
                            cancellationToken
                        );

            var result =
                await
                    response
                        .TryParseResponse<DeleteExecutionResponse>(
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