using System.Threading;
using System.Threading.Tasks;
using Rocket.Api.Contracts.Sessions;

namespace Rocket.Web.Infrastructure.Client
{
    public interface IApiRequestSessions
    {
        Task<MySessionsResponse> GetMySessionsAsync(
            MySessionsRequest request,
            CancellationToken cancellationToken
        );

        Task<RevokeSessionResponse> RevokeSessionAsync(
            string id,
            CancellationToken cancellationToken
        );        
    }
}