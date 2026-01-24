using System.Threading;
using System.Threading.Tasks;
using Rocket.Domain.Core;
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

        Task<CoreWorkflowStep> GetWorkflowStepByIdAsync(
            string workflowStepId,
            string workflowId,
            string userId,
            CancellationToken cancellationToken = default
        );

        Task<CoreWorkflowStep> InsertWorkflowStepAsync(
            CoreWorkflowStep workflowStep,
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
        ) where TWorkflowStep : CoreWorkflowStep;

        Task<bool> DeleteWorkflowStepAsync(
            string userId,
            string workflowId,
            string workflowStepId,
            CancellationToken cancellationToken
        );
    }
}