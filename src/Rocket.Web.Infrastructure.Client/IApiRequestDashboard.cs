using System.Threading;
using System.Threading.Tasks;
using Rocket.Api.Contracts.Dashboard;

namespace Rocket.Web.Infrastructure.Client
{
    public interface IApiRequestDashboard
    {
        Task<FetchDashboardResponse> GetDashboardSnapshotAsync(
            CancellationToken cancellationToken
        );
    }
}