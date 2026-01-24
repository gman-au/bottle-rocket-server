using System.Threading;
using System.Threading.Tasks;
using Rocket.Domain.Core;
using Rocket.Domain.Jobs;

namespace Rocket.Interfaces
{
    public interface IWorkflowExecutionContext
    {
        IIntegrationHook GetApplicableHook(CoreExecutionStep step);

        ExecutionStepArtifact GetInputArtifact();

        void SetCurrentArtifact(ExecutionStepArtifact artifact);

        Task SetRootArtifactAsync(
            string userId,
            string scanId,
            CancellationToken cancellationToken
        );

        Task<T> GetConnectorAsync<T>(
            string userId,
            CoreExecutionStep step,
            CancellationToken cancellationToken
        ) where T : CoreConnector;
    }
}