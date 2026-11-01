using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Rocket.Api.Contracts.Auth;
using Rocket.Domain;

namespace Rocket.Interfaces
{
    public interface ISessionManager
    {
        Task<AccountLoginResponse> CreateSessionAsync(
            string userId,
            bool isAdmin,
            string deviceName,
            string userAgent,
            string ip,
            CancellationToken cancellationToken
        );

        Task<AccountLoginResponse> RefreshAsync(
            string rawRefreshToken,
            CancellationToken cancellationToken
        );

        Task<bool> IsActiveAsync(
            string sessionId,
            CancellationToken cancellationToken
        );

        Task<IEnumerable<TokenSession>> ListAsync(
            string userId,
            CancellationToken cancellationToken
        );

        Task<bool> RevokeAsync(
            string userId,
            string sessionId,
            CancellationToken cancellationToken
        );

        Task RevokeAllAsync(
            string userId,
            string exceptSessionId,
            CancellationToken cancellationToken
        );
    }
}