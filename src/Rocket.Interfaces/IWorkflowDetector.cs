using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Rocket.Interfaces
{
    public interface IWorkflowDetector
    {
        Task DetectAndScheduleWorkflowAsync(
            string batchId, 
            string userId,
            IEnumerable<string> workflowIds,
            CancellationToken cancellationToken
        );
    }
}