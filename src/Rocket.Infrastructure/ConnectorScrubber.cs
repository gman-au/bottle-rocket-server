using System.Threading;
using System.Threading.Tasks;
using Rocket.Interfaces;

namespace Rocket.Infrastructure
{
    public class ConnectorScrubber(IWorkflowStepRepository workflowStepRepository) : IConnectorScrubber
    {
        public async Task<int> ScrubWorkflowsWithConnectorIdAsync(
            string connectorId,
            string userId,
            CancellationToken cancellationToken
        )
        {
            var recordsAffected = 0;

            await
                workflowStepRepository
                    .ScrubConnectorIdFromWorkflowsAsync(
                        connectorId,
                        userId,
                        cancellationToken
                    );
            
            return recordsAffected;
        }
    }
}