using System.Threading;
using System.Threading.Tasks;
using Rocket.Api.Contracts.Dashboard;

namespace Rocket.Web.Client
{
    public interface IApiRequestDashboard
    {
        Task<FetchDashboardResponse> GetDashboardSnapshotAsync(
            CancellationToken cancellationToken
        );
    }
}