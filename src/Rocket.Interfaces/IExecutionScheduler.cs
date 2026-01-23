using System.Threading;
using System.Threading.Tasks;

namespace Rocket.Interfaces
{
    public interface IExecutionScheduler
    {
        Task<string> ScheduleExecutionAsync(
            string workflowId,
            string scanId,
            string userId,
            bool runImmediately,
            CancellationToken cancellationToken
        );
    }
}