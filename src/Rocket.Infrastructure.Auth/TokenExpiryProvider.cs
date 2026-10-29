using System;
using Microsoft.Extensions.Configuration;
using Rocket.Interfaces;

namespace Rocket.Infrastructure.Auth
{
    public class TokenExpiryProvider : ITokenExpiryProvider
    {
        private static readonly TimeSpan DefaultAccessLifetime = TimeSpan.FromMinutes(10);
        private static readonly TimeSpan DefaultRefreshLifetime = TimeSpan.FromDays(30);

        private readonly TimeSpan _accessTokenExpirySeconds;
        private readonly TimeSpan _refreshTokenExpirySeconds;

        public TokenExpiryProvider(
            IConfiguration configuration
        )
        {
            var accessExpiryString =
                configuration["JWT_ACCESS_EXPIRY_SECONDS"];

            _accessTokenExpirySeconds = DefaultAccessLifetime;

            if (!string.IsNullOrEmpty(accessExpiryString))
            {
                if (int.TryParse(
                        accessExpiryString,
                        out var accessExpirySeconds
                    ))
                {
                    if (accessExpirySeconds > 0)
                    {
                        _accessTokenExpirySeconds =
                            TimeSpan
                                .FromSeconds(accessExpirySeconds);
                    }
                }
            }

            var refreshExpiryString =
                configuration["JWT_REFRESH_EXPIRY_SECONDS"];

            _refreshTokenExpirySeconds = DefaultRefreshLifetime;

            if (!string.IsNullOrEmpty(refreshExpiryString))
            {
                if (int.TryParse(
                        refreshExpiryString,
                        out var refreshExpirySeconds
                    ))
                {
                    if (refreshExpirySeconds > 0)
                    {
                        _refreshTokenExpirySeconds =
                            TimeSpan
                                .FromSeconds(refreshExpirySeconds);
                    }
                }
            }
        }

        public TimeSpan GetAccessExpiryLifetime() => _accessTokenExpirySeconds;

        public TimeSpan GetRefreshExpiryLifetime() => _refreshTokenExpirySeconds;
    }
}