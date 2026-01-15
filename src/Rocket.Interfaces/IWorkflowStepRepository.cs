using System.Threading;
using System.Threading.Tasks;
using Rocket.Domain.Workflows;

namespace Rocket.Interfaces
{
    public interface IWorkflowStepRepository
    {
        Task<bool> DeleteWorkflowStepAsync(
            string userId,
            string workflowId,
            string workflowStepId,
            CancellationToken cancellationToken
        );

        Task<BaseWorkflowStep> InsertWorkflowStepAsync(
            BaseWorkflowStep workflowStep,
            string userId,
            string workflowId,
            string parentStepId,
            CancellationToken cancellationToken
        );

        Task<BaseWorkflowStep> GetWorkflowStepByIdAsync(
            string workflowId,
            string userId,
            string workflowStepId,
            CancellationToken cancellationToken = default
        );

        Task<Workflow> GetWorkflowForUserByIdAsync(
            string workflowId,
            string userId,
            CancellationToken cancellationToken
        );
    }
}