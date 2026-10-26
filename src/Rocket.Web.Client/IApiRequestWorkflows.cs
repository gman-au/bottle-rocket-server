using System.Threading;
using System.Threading.Tasks;
using Rocket.Api.Contracts;
using Rocket.Api.Contracts.Workflows;

namespace Rocket.Web.Client
{
    public interface IApiRequestWorkflows
    {
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

        Task<CloneWorkflowResponse> CloneWorkflowAsync(
            CloneWorkflowRequest workflow,
            CancellationToken cancellationToken
        );
    }
}