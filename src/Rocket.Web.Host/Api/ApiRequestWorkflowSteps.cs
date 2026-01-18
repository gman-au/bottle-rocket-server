using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Rocket.Api.Contracts;
using Rocket.Api.Contracts.Workflows;
using Rocket.Web.Host.Extensions;

namespace Rocket.Web.Host.Api
{
    public partial class ApiRequestManager
    {
        public async Task<T> GetWorkflowStepAsync<T>(
            string workflowId,
            string stepId,
            CancellationToken cancellationToken
        ) where T : WorkflowStepSummary
        {
            logger
                .LogInformation("Received Get Workflow Step request");

            var response =
                await
                    authenticatedApiClient
                        .GetAsync(
                            $"/api/workflowSteps/{workflowId}/get/{stepId}",
                            cancellationToken
                        );

            var result =
                await
                    response
                        .TryParseResponse<T>(
                            logger,
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
            logger
                .LogInformation("Received Create Workflow Step request");

            var response =
                await
                    authenticatedApiClient
                        .PostAsJsonAsync(
                            $"/api/workflowSteps/create",
                            request,
                            cancellationToken
                        );

            var result =
                await
                    response
                        .TryParseResponse<CreateWorkflowStepResponse>(
                            logger,
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
            logger
                .LogInformation("Received Update Workflow request");

            var response =
                await
                    authenticatedApiClient
                        .PatchAsJsonAsync(
                            "/api/workflowSteps/update",
                            request,
                            cancellationToken
                        );

            var result =
                await
                    response
                        .TryParseResponse<UpdateWorkflowStepResponse>(
                            logger,
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
            logger
                .LogInformation("Received Delete Workflow Step request");

            var response =
                await
                    authenticatedApiClient
                        .PostAsJsonAsync(
                            $"/api/workflowSteps/delete",
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