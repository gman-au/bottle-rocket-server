using System.Threading;
using System.Threading.Tasks;
using Rocket.Api.Contracts;
using Rocket.Api.Contracts.Workflows;

namespace Rocket.Web.Infrastructure.Client
{
    public interface IApiRequestWorkflowSteps
    {
        Task<T> GetWorkflowStepAsync<T>(
            string workflowId,
            string stepId,
            CancellationToken cancellationToken
        ) where T : WorkflowStepSummary;

        Task<CreateWorkflowStepResponse> CreateWorkflowStepAsync<T>(
            CreateWorkflowStepRequest<T> request,
            CancellationToken cancellationToken
        ) where T : WorkflowStepSummary;

        Task<UpdateWorkflowStepResponse> UpdateWorkflowStepAsync<T>(
            UpdateWorkflowStepRequest<T> request,
            CancellationToken cancellationToken
        ) where T : WorkflowStepSummary;

        Task<ApiResponse> DeleteWorkflowStepByIdAsync(
            DeleteWorkflowStepRequest request,
            CancellationToken cancellationToken
        );
    }
}