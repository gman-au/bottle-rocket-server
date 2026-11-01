using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Rocket.Api.Contracts;
using Rocket.Api.Contracts.Workflows;
using Rocket.Interfaces;
using Rocket.Web.Infrastructure.Client.Extensions;

namespace Rocket.Web.Infrastructure.Client
{
    internal class ApiRequestWorkflowSteps(
        ILogger<ApiRequestWorkflowSteps> logger,
        IJsonResolverInstanceProvider jsonResolverInstanceProvider,
        IAuthenticatedApiClient authenticatedApiClient
    ) : ApiRequestBase(
        logger,
        jsonResolverInstanceProvider,
        authenticatedApiClient
    ), IApiRequestWorkflowSteps
    {
        public async Task<T> GetWorkflowStepAsync<T>(
            string workflowId,
            string stepId,
            CancellationToken cancellationToken
        ) where T : WorkflowStepSummary
        {
            Logger
                .LogInformation("Received Get Workflow Step request");

            var response =
                await
                    AuthenticatedApiClient
                        .GetAsync(
                            $"/api/workflowSteps/{workflowId}/get/{stepId}",
                            cancellationToken
                        );

            var result =
                await
                    response
                        .TryParseResponse<T>(
                            JsonSerializerOptions,
                            Logger,
                            cancellationToken
                        );

            EnsureApiSuccessStatusCode(result);
            EnsureHttpSuccessStatusCode(response);

            return result;
        }

        public async Task<CreateWorkflowStepResponse> CreateWorkflowStepAsync<T>(
            CreateWorkflowStepRequest<T> request,
            CancellationToken cancellationToken
        ) where T : WorkflowStepSummary
        {
            Logger
                .LogInformation("Received Create Workflow Step request");

            var response =
                await
                    AuthenticatedApiClient
                        .PostAsJsonAsync(
                            "/api/workflowSteps/create",
                            request,
                            cancellationToken
                        );

            var result =
                await
                    response
                        .TryParseResponse<CreateWorkflowStepResponse>(
                            JsonSerializerOptions,
                            Logger,
                            cancellationToken
                        );

            EnsureApiSuccessStatusCode(result);
            EnsureHttpSuccessStatusCode(response);

            return result;
        }
        
        public async Task<UpdateWorkflowStepResponse> UpdateWorkflowStepAsync<T>(
            UpdateWorkflowStepRequest<T> request,
            CancellationToken cancellationToken
        ) where T : WorkflowStepSummary
        {
            Logger
                .LogInformation("Received Update Workflow request");

            var response =
                await
                    AuthenticatedApiClient
                        .PatchAsJsonAsync(
                            "/api/workflowSteps/update",
                            request,
                            cancellationToken
                        );

            var result =
                await
                    response
                        .TryParseResponse<UpdateWorkflowStepResponse>(
                            JsonSerializerOptions,
                            Logger,
                            cancellationToken
                        );

            EnsureApiSuccessStatusCode(result);
            EnsureHttpSuccessStatusCode(response);

            return result;
        }

        public async Task<ApiResponse> DeleteWorkflowStepByIdAsync(
            DeleteWorkflowStepRequest request,
            CancellationToken cancellationToken
        )
        {
            Logger
                .LogInformation("Received Delete Workflow Step request");

            var response =
                await
                    AuthenticatedApiClient
                        .PostAsJsonAsync(
                            "/api/workflowSteps/delete",
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