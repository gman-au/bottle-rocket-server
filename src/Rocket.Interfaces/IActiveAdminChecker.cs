using System.Threading;
using System.Threading.Tasks;

namespace Rocket.Interfaces
{
    public interface IActiveAdminChecker
    {
        Task<bool> PerformAsync(
            string proposedUserId,
            bool? proposedIsActive,
            bool? proposedIsAdmin,
            CancellationToken cancellationToken
        );
    }
}