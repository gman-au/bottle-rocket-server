using System.Threading;
using System.Threading.Tasks;
using Rocket.Domain.Executions;
using Rocket.Domain.Jobs;

namespace Rocket.Interfaces
{
    public interface IIntegrationHook
    {
        bool IsApplicable(BaseExecutionStep step);

        Task<ExecutionStepArtifact> ProcessAsync(
            ExecutionStepArtifact artifact,
            CancellationToken cancellationToken
        );
    }
}