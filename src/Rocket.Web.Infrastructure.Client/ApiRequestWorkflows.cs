using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Rocket.Api.Contracts;
using Rocket.Api.Contracts.Workflows;
using Rocket.Interfaces;
using Rocket.Web.Infrastructure.Client.Extensions;

namespace Rocket.Web.Infrastructure.Client
{
    internal class ApiRequestWorkflows(
        ILogger<ApiRequestWorkflows> logger,
        IJsonResolverInstanceProvider jsonResolverInstanceProvider,
        IAuthenticatedApiClient authenticatedApiClient
    ) : ApiRequestBase(
        logger,
        jsonResolverInstanceProvider,
        authenticatedApiClient
    ), IApiRequestWorkflows
    {
        public async Task<FetchWorkflowsResponse> GetWorkflowsAsync(
            FetchWorkflowsRequest request,
            CancellationToken cancellationToken
        )
        {
            Logger
                .LogInformation("Received get workflows request");

            var response =
                await
                    AuthenticatedApiClient
                        .PostAsJsonAsync(
                            "/api/workflows/fetch",
                            request,
                            cancellationToken
                        );

            var result =
                await
                    response
                        .TryParseResponse<FetchWorkflowsResponse>(
                            JsonSerializerOptions,
                            Logger,
                            cancellationToken
                        );

            EnsureApiSuccessStatusCode(result);
            EnsureHttpSuccessStatusCode(response);

            return result;
        }

        public async Task<WorkflowSummary> GetWorkflowByIdAsync(
            string id,
            CancellationToken cancellationToken
        )
        {
            Logger
                .LogInformation("Received Get Workflow request");

            var response =
                await
                    AuthenticatedApiClient
                        .GetAsync(
                            $"/api/workflows/get/{id}",
                            cancellationToken
                        );

            var result =
                await
                    response
                        .TryParseResponse<WorkflowSummary>(
                            JsonSerializerOptions,
                            Logger,
                            cancellationToken
                        );

            EnsureApiSuccessStatusCode(result);
            EnsureHttpSuccessStatusCode(response);

            return result;
        }

        public async Task<UpdateWorkflowResponse> UpdateWorkflowAsync(
            MyWorkflowSummary workflow,
            CancellationToken cancellationToken
        )
        {
            Logger
                .LogInformation("Received Update Workflow request");

            var response =
                await
                    AuthenticatedApiClient
                        .PatchAsJsonAsync(
                            "/api/workflows/update",
                            workflow,
                            cancellationToken
                        );

            var result =
                await
                    response
                        .TryParseResponse<UpdateWorkflowResponse>(
                            JsonSerializerOptions,
                            Logger,
                            cancellationToken
                        );

            EnsureApiSuccessStatusCode(result);
            EnsureHttpSuccessStatusCode(response);

            return result;
        }

        public async Task<CreateWorkflowResponse> CreateWorkflowAsync(
            CreateWorkflowRequest workflow,
            CancellationToken cancellationToken
        )
        {
            Logger
                .LogInformation("Received Create Workflow request");

            var response =
                await
                    AuthenticatedApiClient
                        .PostAsJsonAsync(
                            "/api/workflows/create",
                            workflow,
                            cancellationToken
                        );

            var result =
                await
                    response
                        .TryParseResponse<CreateWorkflowResponse>(
                            JsonSerializerOptions,
                            Logger,
                            cancellationToken
                        );

            EnsureApiSuccessStatusCode(result);
            EnsureHttpSuccessStatusCode(response);

            return result;
        }

        public async Task<CloneWorkflowResponse> CloneWorkflowAsync(
            CloneWorkflowRequest workflow,
            CancellationToken cancellationToken
        )
        {
            Logger
                .LogInformation("Received Clone Workflow request");

            var response =
                await
                    AuthenticatedApiClient
                        .PostAsJsonAsync(
                            "/api/workflows/clone",
                            workflow,
                            cancellationToken
                        );

            var result =
                await
                    response
                        .TryParseResponse<CloneWorkflowResponse>(
                            JsonSerializerOptions,
                            Logger,
                            cancellationToken
                        );

            EnsureApiSuccessStatusCode(result);
            EnsureHttpSuccessStatusCode(response);

            return result;
        }

        public async Task<ApiResponse> DeleteWorkflowByIdAsync(
            string id,
            CancellationToken cancellationToken
        )
        {
            Logger
                .LogInformation("Received Delete Workflow request");

            var response =
                await
                    AuthenticatedApiClient
                        .DeleteAsync(
                            $"/api/workflows/{id}",
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