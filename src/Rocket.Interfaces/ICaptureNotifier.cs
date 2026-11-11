using System.Threading;
using System.Threading.Tasks;

namespace Rocket.Interfaces
{
    public interface ICaptureNotifier
    {
        Task NotifyNewCaptureAsync(string userId, CancellationToken cancellationToken);

        Task NotifyNewExecutionUpdateAsync(string userId, CancellationToken cancellationToken);

        Task NotifyNewExecutionElementUpdateAsync(string userId, string scanId, int executionStatus, CancellationToken cancellationToken);
        
        Task NotifyConnectorUpdateAsync(string userId, bool success, CancellationToken cancellationToken);
        
        Task NotifyScanDeleteAsync(string userId, CancellationToken cancellationToken);
        
        Task NotifyExecutionDeleteAsync(string userId, CancellationToken cancellationToken);
    }
}