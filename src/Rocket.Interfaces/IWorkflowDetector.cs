using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Rocket.Interfaces
{
    public interface IWorkflowDetector
    {
        Task DetectAndScheduleWorkflowAsync(
            string scanId, 
            string userId,
            string vendor,
            IEnumerable<string> workflowIds,
            CancellationToken cancellationToken
        );
    }
}