using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Rocket.Api.Contracts;
using Rocket.Api.Contracts.Workflows;
using Rocket.Web.Client.Extensions;

namespace Rocket.Web.Client
{
    public partial class ApiRequestManager
    {
        public async Task<FetchWorkflowsResponse> GetWorkflowsAsync(
            FetchWorkflowsRequest request,
            CancellationToken cancellationToken
        )
        {
            logger
                .LogInformation("Received get workflows request");

            var response =
                await
                    authenticatedApiClient
                        .PostAsJsonAsync(
                            "/api/workflows/fetch",
                            request,
                            cancellationToken
                        );

            var result =
                await
                    response
                        .TryParseResponse<FetchWorkflowsResponse>(
                            logger,
                            cancellationToken
                        );

            EnsureApiSuccessStatusCode(result);
            EnsureHttpSuccessStatusCode(response);

            return result;
        }

        public async Task<MyWorkflowSummary> GetWorkflowByIdAsync(
            string id,
            CancellationToken cancellationToken
        )
        {
            logger
                .LogInformation("Received Get Workflow request");

            var response =
                await
                    authenticatedApiClient
                        .GetAsync(
                            $"/api/workflows/get/{id}",
                            cancellationToken
                        );

            var result =
                await
                    response
                        .TryParseResponse<MyWorkflowSummary>(
                            logger,
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
            logger
                .LogInformation("Received Update Workflow request");

            var response =
                await
                    authenticatedApiClient
                        .PatchAsJsonAsync(
                            "/api/workflows/update",
                            workflow,
                            cancellationToken
                        );

            var result =
                await
                    response
                        .TryParseResponse<UpdateWorkflowResponse>(
                            logger,
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
            logger
                .LogInformation("Received Create Workflow request");

            var response =
                await
                    authenticatedApiClient
                        .PostAsJsonAsync(
                            "/api/workflows/create",
                            workflow,
                            cancellationToken
                        );

            var result =
                await
                    response
                        .TryParseResponse<CreateWorkflowResponse>(
                            logger,
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
            logger
                .LogInformation("Received Delete Workflow request");

            var response =
                await
                    authenticatedApiClient
                        .DeleteAsync(
                            $"/api/workflows/{id}",
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