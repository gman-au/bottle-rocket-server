using System.Threading;
using System.Threading.Tasks;

namespace Rocket.Interfaces
{
    public interface IWorkflowExecutionManager
    {
        Task<bool> StartExecutionAsync(
            string executionId,
            string userId,
            CancellationToken cancellationToken
        );

        Task<bool> CancelExecutionAsync(string executionId);
    }
}