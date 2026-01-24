using System.Threading;
using System.Threading.Tasks;
using Rocket.Domain.Core;
using Rocket.Domain.Executions;
using Rocket.Domain.Jobs;

namespace Rocket.Interfaces
{
    public interface IIntegrationHook
    {
        bool IsApplicable(CoreExecutionStep step);

        Task<ExecutionStepArtifact> ProcessAsync(
            IWorkflowExecutionContext context,
            CoreExecutionStep step,
            string userId,
            CancellationToken cancellationToken
        );
    }
}