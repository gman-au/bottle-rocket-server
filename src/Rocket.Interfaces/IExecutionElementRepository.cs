using System.Threading;
using System.Threading.Tasks;
using Rocket.Domain.Executions;

namespace Rocket.Interfaces
{
    public interface IExecutionElementRepository : IMigratableRepository
    {
        Task<ExecutionElement> GetExecutionElementByScanIdAsync(
            string userId,
            string executionId,
            string scanId,
            CancellationToken cancellationToken
        );

        Task<Execution> UpdateExecutionElementAsync(
            string executionElementId,
            string executionId,
            string userId,
            ExecutionElement updatedElement,
            CancellationToken cancellationToken
        );
    }
}