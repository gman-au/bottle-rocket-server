using System.Threading;
using System.Threading.Tasks;
using Rocket.Domain;
using Rocket.Domain.Connectors;
using Rocket.Domain.Executions;
using Rocket.Domain.Jobs;

namespace Rocket.Interfaces
{
    public interface IWorkflowExecutionContext
    {
        IIntegrationHook GetApplicableHook(BaseExecutionStep step);

        ExecutionStepArtifact GetInputArtifact();

        void SetCurrentArtifact(ExecutionStepArtifact artifact);

        Task SetRootArtifactAsync(
            string userId,
            string scanId,
            CancellationToken cancellationToken
        );

        Task<T> GetConnectorAsync<T>(
            string userId,
            BaseExecutionStep step,
            CancellationToken cancellationToken
        ) where T : BaseConnector;
    }
}