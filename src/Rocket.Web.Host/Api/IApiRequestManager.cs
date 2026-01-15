using System.Threading;
using System.Threading.Tasks;
using Rocket.Api.Contracts;
using Rocket.Api.Contracts.Connectors;
using Rocket.Api.Contracts.Scans;
using Rocket.Api.Contracts.Users;
using Rocket.Api.Contracts.Workflows;
using Rocket.Dropbox.Contracts;

namespace Rocket.Web.Host.Api
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

        Task<MyWorkflowSummary> GetWorkflowByIdAsync(
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

        Task<CreateDropboxConnectorResponse> CreateDropboxConnectorAsync(
            CreateDropboxConnectorRequest connector,
            CancellationToken cancellationToken
        );

        public Task<ApiResponse> FinalizeDropboxConnectorAsync(
            FinalizeDropboxConnectorRequest request,
            CancellationToken cancellationToken
        );

        public Task<CreateWorkflowStepResponse> CreateDropboxUploadFileStepAsync(
            CreateWorkflowStepRequest<DropboxUploadStepSpecifics> request,
            CancellationToken cancellationToken
        );
    }
}