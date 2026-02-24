using System.Threading;
using System.Threading.Tasks;

namespace Rocket.Interfaces
{
    public interface ICaptureSweeper
    {
        Task PerformAsync(CancellationToken cancellationToken);
    }
}