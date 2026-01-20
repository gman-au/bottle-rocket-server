using System.Threading;
using System.Threading.Tasks;
using Rocket.Domain.Connectors;
using Rocket.Domain.Executions;
using Rocket.Domain.Jobs;

namespace Rocket.Interfaces
{
    public interface IWorkflowExecutionContext
    {
        IIntegrationHook GetApplicableHook(BaseExecutionStep step);

        ExecutionStepArtifact GetInputArtifact();

        Task<BaseConnector> GetConnectorAsync(
            string userId,
            BaseExecutionStep step,
            CancellationToken cancellationToken
        );
    }
}