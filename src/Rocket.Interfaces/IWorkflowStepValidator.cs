using System.Threading;
using System.Threading.Tasks;

namespace Rocket.Interfaces
{
    public interface IWorkflowStepValidator
    {
        Task ValidateAsync(
            string workflowId,
            string parentStepId,
            string userId,
            int childInputType,
            CancellationToken cancellationToken
        );
    }
}