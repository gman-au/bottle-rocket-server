using System.Threading;
using System.Threading.Tasks;
using Rocket.Api.Contracts;
using Rocket.Api.Contracts.Connectors;
using Rocket.Api.Contracts.Executions;
using Rocket.Api.Contracts.Scans;
using Rocket.Api.Contracts.Users;
using Rocket.Api.Contracts.Workflows;
using Rocket.Dropbox.Contracts;
using Rocket.Notion.Contracts;

namespace Rocket.Web.Client
{
    public interface IApiRequestManager
    {
        Task<MyScansResponse> GetMyScansAsync(
            MyScansRequest request,
            CancellationToken cancellationToken
        );

        Task<ScanSpecifics> GetMyScanAsync(
            string id,
            CancellationToken cancellationToken
        );

        Task<UserSpecifics> GetUserByIdAsync(
            string id,
            CancellationToken cancellationToken
        );

        Task<UpdateUserResponse> UpdateUserAsync(
            UserSpecifics user,
            CancellationToken cancellationToken
        );

        Task<CreateUserResponse> CreateUserAsync(
            CreateUserRequest user,
            CancellationToken cancellationToken
        );

        Task<StartupPhaseResponse> GetStartupPhaseAsync(CancellationToken cancellationToken);

        Task<FetchUsersResponse> GetUsersAsync(
            FetchUsersRequest request,
            CancellationToken cancellationToken
        );

        Task<FetchWorkflowsResponse> GetWorkflowsAsync(
            FetchWorkflowsRequest request,
            CancellationToken cancellationToken
        );

        Task<WorkflowSummary> GetWorkflowByIdAsync(
            string id,
            CancellationToken cancellationToken
        );

        Task<UpdateWorkflowResponse> UpdateWorkflowAsync(
            MyWorkflowSummary workflow,
            CancellationToken cancellationToken
        );

        Task<ApiResponse> DeleteWorkflowByIdAsync(
            string id,
            CancellationToken cancellationToken
        );

        Task<CreateWorkflowResponse> CreateWorkflowAsync(
            CreateWorkflowRequest workflow,
            CancellationToken cancellationToken
        );

        Task<FetchConnectorsResponse> GetMyConnectorsAsync(
            FetchConnectorsRequest request,
            CancellationToken baseCancellationToken
        );

        Task<ApiResponse> DeleteConnectorByIdAsync(
            string id,
            CancellationToken cancellationToken
        );

        public Task<CreateConnectorResponse<T>> CreateConnectorAsync<T>(
            CreateConnectorRequest<T> request,
            CancellationToken cancellationToken
        ) where T : ConnectorSummary;

        public Task<T> GetWorkflowStepAsync<T>(
            string workflowId,
            string stepId,
            CancellationToken cancellationToken
        ) where T : WorkflowStepSummary;

        public Task<CreateWorkflowStepResponse> CreateWorkflowStepAsync<T>(
            CreateWorkflowStepRequest<T> request,
            CancellationToken cancellationToken
        ) where T : WorkflowStepSummary;

        public Task<UpdateWorkflowStepResponse> UpdateWorkflowStepAsync<T>(
            UpdateWorkflowStepRequest<T> request,
            CancellationToken cancellationToken
        ) where T : WorkflowStepSummary;

        Task<ApiResponse> DeleteWorkflowStepByIdAsync(
            DeleteWorkflowStepRequest request,
            CancellationToken cancellationToken
        );

        Task<FetchExecutionsResponse> GetExecutionsAsync(
            FetchExecutionsRequest request,
            CancellationToken cancellationToken
        );

        Task<ExecutionSummary> GetExecutionByIdAsync(
            string id,
            CancellationToken cancellationToken
        );

        Task<CreateExecutionResponse> CreateExecutionAsync(
            CreateExecutionRequest request,
            CancellationToken cancellationToken
        );

        Task<StartExecutionResponse> StartExecutionAsync(
            string executionId,
            CancellationToken cancellationToken
        );

        Task<CancelExecutionResponse> CancelExecutionAsync(
            string executionId,
            CancellationToken cancellationToken
        );

        public Task<ApiResponse> FinalizeDropboxConnectorAsync(
            FinalizeDropboxConnectorRequest request,
            CancellationToken cancellationToken
        );

        public Task<GetAllNotionParentNotesResponse> GetNotionParentNotesAsync(
            GetAllNotionParentNotesRequest request,
            CancellationToken cancellationToken
        );
    }
}