using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Rocket.Domain.Connectors;
using Rocket.Domain.Executions;
using Rocket.Domain.Jobs;
using Rocket.Interfaces;

namespace Rocket.Jobs.Service
{
    public class WorkflowExecutionContext(
        IEnumerable<IIntegrationHook> hooks,
        IConnectorRepository connectorRepository
    )
        : IWorkflowExecutionContext
    {
        public ExecutionStepArtifact GetInputArtifact()
        {
            return null;
        }

        public IIntegrationHook GetApplicableHook(BaseExecutionStep step)
        {
            return
                hooks
                    .FirstOrDefault(o => o.IsApplicable(step));
        }

        public async Task<BaseConnector> GetConnectorAsync(
            string userId,
            BaseExecutionStep step,
            CancellationToken cancellationToken
        )
        {
            var connectorId = step?.ConnectorId;

            var connector =
                await
                    connectorRepository
                        .GetConnectorByIdAsync<BaseConnector>(
                            userId,
                            connectorId,
                            cancellationToken
                        );

            return connector;
        }
    }
}