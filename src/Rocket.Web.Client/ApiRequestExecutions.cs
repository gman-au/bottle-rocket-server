using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Rocket.Api.Contracts.Executions;
using Rocket.Web.Client.Extensions;

namespace Rocket.Web.Client
{
    public partial class ApiRequestManager
    {
        public async Task<ExecutionSummary> GetExecutionByIdAsync(
            string id,
            CancellationToken cancellationToken
        )
        {
            logger
                .LogInformation("Received Get Execution request");

            var response =
                await
                    authenticatedApiClient
                        .GetAsync(
                            $"/api/executions/get/{id}",
                            cancellationToken
                        );

            var result =
                await
                    response
                        .TryParseResponse<ExecutionSummary>(
                            logger,
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
            logger
                .LogInformation("Received Create Execution request");

            var response =
                await
                    authenticatedApiClient
                        .PostAsJsonAsync(
                            "/api/executions/create",
                            workflow,
                            cancellationToken
                        );

            var result =
                await
                    response
                        .TryParseResponse<CreateExecutionResponse>(
                            logger,
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
            logger
                .LogInformation("Received Start Execution request");

            var response =
                await
                    authenticatedApiClient
                        .PutAsync(
                            $"/api/executions/start/{executionId}",
                            null,
                            cancellationToken
                        );

            var result =
                await
                    response
                        .TryParseResponse<StartExecutionResponse>(
                            logger,
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
            logger
                .LogInformation("Received Cancel Execution request");

            var response =
                await
                    authenticatedApiClient
                        .PutAsync(
                            $"/api/executions/cancel/{executionId}",
                            null,
                            cancellationToken
                        );

            var result =
                await
                    response
                        .TryParseResponse<CancelExecutionResponse>(
                            logger,
                            cancellationToken
                        );

            EnsureApiSuccessStatusCode(result);
            EnsureHttpSuccessStatusCode(response);

            return result;
        }
    }
}