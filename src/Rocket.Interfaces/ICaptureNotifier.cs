using System.Threading;
using System.Threading.Tasks;

namespace Rocket.Interfaces
{
    public interface ICaptureNotifier
    {
        Task NotifyNewCaptureAsync(string userId, CancellationToken cancellationToken);
    }
}