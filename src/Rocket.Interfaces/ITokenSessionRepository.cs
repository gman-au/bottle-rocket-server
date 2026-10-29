using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Rocket.Domain;

namespace Rocket.Interfaces
{
    public interface ITokenSessionRepository
    {
        Task<TokenSession> InsertTokenAsync(
            TokenSession tokenSession,
            CancellationToken cancellationToken
        );

        Task<TokenSession> RotateAsync(
            string oldHash,
            string newHash,
            string newPrefix,
            DateTime now,
            DateTime newExpiresAt,
            CancellationToken cancellationToken
        );

        Task RevokeIfReplayedAsync(
            string oldHash,
            DateTime now,
            TimeSpan grace,
            CancellationToken cancellationToken
        );

        Task<IEnumerable<TokenSession>> GetActiveBySessionAsync(
            string sessionId,
            DateTime now,
            CancellationToken cancellationToken
        );

        Task<IEnumerable<TokenSession>> GetActiveByUserAsync(
            string sessionId,
            DateTime now,
            CancellationToken cancellationToken
        );

        Task<bool> RevokeAsync(
            string userId,
            string sessionId,
            DateTime now,
            CancellationToken cancellationToken
        );

        Task RevokeAllAsync(
            string userId,
            string exceptSessionId,
            DateTime now,
            CancellationToken cancellationToken
        );
    }
}