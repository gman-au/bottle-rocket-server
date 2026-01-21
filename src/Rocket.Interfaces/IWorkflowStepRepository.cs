using System.Threading;
using System.Threading.Tasks;
using Rocket.Domain.Workflows;

namespace Rocket.Interfaces
{
    public interface IWorkflowStepRepository
    {
        Task<Workflow> GetWorkflowByIdAsync(
            string workflowId,
            string userId,
            CancellationToken cancellationToken
        );

        Task<BaseWorkflowStep> GetWorkflowStepByIdAsync(
            string workflowStepId,
            string workflowId,
            string userId,
            CancellationToken cancellationToken = default
        );

        Task<BaseWorkflowStep> InsertWorkflowStepAsync(
            BaseWorkflowStep workflowStep,
            string userId,
            string workflowId,
            string parentStepId,
            CancellationToken cancellationToken
        );

        Task<Workflow> UpdateWorkflowStepAsync<TWorkflowStep>(
            string workflowStepId,
            string workflowId,
            string userId,
            TWorkflowStep updatedWorkflowStep,
            CancellationToken cancellationToken
        ) where TWorkflowStep : BaseWorkflowStep;

        Task<bool> DeleteWorkflowStepAsync(
            string userId,
            string workflowId,
            string workflowStepId,
            CancellationToken cancellationToken
        );
    }
}