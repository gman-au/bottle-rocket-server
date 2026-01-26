using System.Threading;
using System.Threading.Tasks;

namespace Rocket.Interfaces
{
    public interface IConnectorScrubber
    {
        Task<int> ScrubWorkflowsWithConnectorIdAsync(
            string connectorId,
            string userId,
            CancellationToken cancellationToken
        );
    }
}