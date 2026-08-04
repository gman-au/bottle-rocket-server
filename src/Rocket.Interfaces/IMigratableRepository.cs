using System.Threading;
using System.Threading.Tasks;

namespace Rocket.Interfaces
{
    public interface IMigratableRepository
    {
        Task SafelyDropColumnAsync(
            string columnName,
            CancellationToken cancellationToken
        );
    }
}