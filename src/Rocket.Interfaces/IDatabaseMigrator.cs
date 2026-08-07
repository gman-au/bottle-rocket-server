using System.Threading;
using System.Threading.Tasks;

namespace Rocket.Interfaces
{
    public interface IDatabaseMigrator
    {
        Task ApplyMigrationsAsync(
            CancellationToken cancellationToken
        );
    }
}