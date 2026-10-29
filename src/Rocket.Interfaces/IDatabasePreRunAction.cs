using System.Threading;
using System.Threading.Tasks;

namespace Rocket.Interfaces
{
    public interface IDatabasePreRunAction
    {
        Task RunAsync(CancellationToken cancellationToken);
    }
}