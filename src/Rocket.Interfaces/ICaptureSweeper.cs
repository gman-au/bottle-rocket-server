using System.Threading;
using System.Threading.Tasks;

namespace Rocket.Interfaces
{
    public interface ICaptureSweeper
    {
        Task<(int, int)> PerformAsync(CancellationToken cancellationToken);
    }
}