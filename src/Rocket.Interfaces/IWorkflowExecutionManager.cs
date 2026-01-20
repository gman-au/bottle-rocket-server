using System.Threading.Tasks;

namespace Rocket.Interfaces
{
    public interface IWorkflowExecutionManager
    {
        Task<bool> StartExecutionAsync(string executionId);

        Task<bool> CancelExecutionAsync(string executionId);
    }
}