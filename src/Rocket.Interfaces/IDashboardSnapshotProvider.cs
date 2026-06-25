using System.Threading;
using System.Threading.Tasks;
using Rocket.Domain.Dashboard;

namespace Rocket.Interfaces
{
    public interface IDashboardSnapshotProvider
    {
        Task<DashboardSnapshot> GetSnapshotForUserAsync(
            string userId,
            CancellationToken cancellationToken
        );

        void MarkAsDirty(
            string userId
        );
    }
}