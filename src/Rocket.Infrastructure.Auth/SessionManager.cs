using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using Rocket.Api.Contracts.Auth;
using Rocket.Domain;
using Rocket.Interfaces;

namespace Rocket.Infrastructure.Auth
{
    public class SessionManager(
        IJwtTokenGenerator jwtTokenGenerator,
        ITokenSessionRepository tokenSessionRepository
    ) : ISessionManager
    {
        private static readonly TimeSpan AccessLifetime = TimeSpan.FromMinutes(10);
        private static readonly TimeSpan RefreshLifetime = TimeSpan.FromDays(30);
        private static readonly TimeSpan ReuseGracePeriod = TimeSpan.FromSeconds(30);

        public async Task<AccountLoginResponse> CreateSessionAsync(
            string userId,
            bool isAdmin,
            string deviceName,
            string userAgent,
            string ip,
            CancellationToken cancellationToken
        )
        {
            var (raw, hash) = GenerateRefreshToken();

            var now = DateTime.UtcNow;

            var session =
                new TokenSession
                {
                    UserId = userId,
                    IsAdmin = isAdmin,
                    UserAgent = userAgent,
                    IpAddress = ip,
                    Hash = hash,
                    TokenPrefix = raw[..8],
                    CreatedAt = now,
                    LastLoginAt = now,
                    ExpiresAt = now + RefreshLifetime,
                    IsActive = true,
                };

            await
                tokenSessionRepository
                    .InsertTokenAsync(
                        session,
                        cancellationToken
                    );

            return
                Issue(
                    session,
                    raw
                );
        }

        public Task<IEnumerable<TokenSession>> ListAsync(
            string userId,
            CancellationToken cancellationToken
        ) =>
            tokenSessionRepository
                .GetActiveByUserAsync(
                    userId,
                    DateTime.UtcNow,
                    cancellationToken
                );

        public Task<bool> RevokeAsync(
            string userId,
            string sessionId,
            CancellationToken cancellationToken
        ) =>
            tokenSessionRepository
                .RevokeAsync(
                    userId,
                    sessionId,
                    DateTime.UtcNow,
                    cancellationToken
                );

        public Task RevokeAllAsync(
            string userId,
            string exceptSessionId,
            CancellationToken cancellationToken
        ) =>
            tokenSessionRepository
                .RevokeAllAsync(
                    userId,
                    exceptSessionId,
                    DateTime.UtcNow,
                    cancellationToken
                );

        public async Task<AccountLoginResponse> RefreshAsync(
            string rawRefreshToken,
            CancellationToken cancellationToken
        )
        {
            var oldHash = HashToken(rawRefreshToken);
            var (newRaw, newHash) = GenerateRefreshToken();
            var now = DateTime.UtcNow;

            var session =
                await
                    tokenSessionRepository
                        .RotateAsync(
                            oldHash,
                            newHash,
                            newRaw[..8],
                            now,
                            now + RefreshLifetime,
                            cancellationToken
                        );

            if (session != null)
                return Issue(
                    session,
                    newRaw
                );

            // Unknown, expired or revoked. If it's an *old* token being replayed
            // (outside the grace period), assume it was stolen and kill the session.
            await
                tokenSessionRepository
                    .RevokeIfReplayedAsync(
                        oldHash,
                        now,
                        ReuseGracePeriod,
                        cancellationToken
                    );

            return null;
        }

        public async Task<bool> IsActiveAsync(
            string sessionId,
            CancellationToken cancellationToken
        )
        {
            var count =
                await
                    tokenSessionRepository
                        .GetActiveBySessionAsync(
                            sessionId,
                            DateTime.UtcNow,
                            cancellationToken
                        );

            return
                count
                    .Any();
        }

        private AccountLoginResponse Issue(
            TokenSession session,
            string rawRefresh
        )
        {
            var expiresAt = DateTime.UtcNow + AccessLifetime;

            var access =
                jwtTokenGenerator
                    .CreateAccessToken(
                        session.UserId,
                        session.Id,
                        session.IsAdmin,
                        expiresAt
                    );

            return
                new AccountLoginResponse
                {
                    AccessToken = access,
                    RefreshToken = rawRefresh,
                    SessionId = session.Id,
                    AccessTokenExpiresAt = expiresAt
                };
        }

        private static (string raw, string hash) GenerateRefreshToken()
        {
            var raw =
                WebEncoders
                    .Base64UrlEncode(
                        RandomNumberGenerator
                            .GetBytes(
                                32
                            )
                    );

            return (raw, HashToken(raw));
        }

        private static string HashToken(
            string raw
        ) =>
            Convert
                .ToHexString(
                    SHA256
                        .HashData(
                            Encoding
                                .UTF8
                                .GetBytes(raw)
                        )
                );
    }
}