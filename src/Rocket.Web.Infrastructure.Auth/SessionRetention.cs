using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.Extensions.Logging;
using Rocket.Domain.Utils;
using Rocket.Interfaces;
using Rocket.Web.Infrastructure.Client;

namespace Rocket.Web.Infrastructure.Auth
{
    public class SessionRetention : ISessionRetention
    {
        private readonly ILogger<SessionRetention> _logger;

        private readonly ProtectedSessionStorage _sessionStorage;

        private readonly IThemeService _themeService;
        
        private bool _initialized;

        private string _cachedAuthHeader;
        private string _cachedCurrentLanguage;
        private string _cachedRefreshToken;
        private string _cachedRole;
        private string _cachedUsername;
        private DateTime? _cachedExpiresAt;

        public SessionRetention(
            ProtectedSessionStorage sessionStorage,
            IThemeService themeService,
            ILogger<SessionRetention> logger
        )
        {
            _sessionStorage = sessionStorage;
            _themeService = themeService;
            _logger = logger;
        }

        public string CachedAuthHeader => _cachedAuthHeader;
        
        public string CachedRefreshToken => _cachedRefreshToken;
        
        public string CachedUserName => _cachedUsername;
        
        public string CachedRole => _cachedRole;
        
        public string CachedCurrentLanguage => _cachedCurrentLanguage;
        
        public DateTime? CachedExpiresAt => _cachedExpiresAt;

        public bool IsInitialized() => _initialized;

        public void SetCurrentLanguage(
            string language
        )
        {
            _cachedCurrentLanguage = language;
        }

        public async Task RetainSessionTokensAsync(
            string accessToken,
            string refreshToken,
            DateTime accessTokenExpiresAt,
            CancellationToken cancellationToken
        )
        {
            _cachedAuthHeader = $"{DomainConstants.Bearer} {accessToken}";

            _cachedRefreshToken = refreshToken;

            _cachedExpiresAt =
                accessTokenExpiresAt.Kind == DateTimeKind.Unspecified
                    ? DateTime.SpecifyKind(
                        accessTokenExpiresAt,
                        DateTimeKind.Utc
                    )
                    : accessTokenExpiresAt.ToUniversalTime();

            try
            {
                await
                    _sessionStorage
                        .SetAsync(
                            DomainConstants.AuthHeaderKey,
                            _cachedAuthHeader
                        );
                await
                    _sessionStorage
                        .SetAsync(
                            DomainConstants.RefreshTokenKey,
                            _cachedRefreshToken
                        );
                await
                    _sessionStorage
                        .SetAsync(
                            DomainConstants.AccessTokenExpiresAtKey,
                            _cachedExpiresAt.Value
                        );
            }
            catch (InvalidOperationException ex) when (ex.Message.Contains("prerendering"))
            {
                _logger
                    .LogDebug("Cannot save to session storage during prerendering");
            }
        }

        public async Task RetainSessionUserAsync(
            string userName,
            string role,
            string currentLanguage,
            CancellationToken cancellationToken
        )
        {
            _cachedUsername = userName;
            _cachedRole = role;
            _cachedCurrentLanguage = currentLanguage;

            try
            {
                await
                    _sessionStorage
                        .SetAsync(
                            DomainConstants.UsernameKey,
                            userName
                        );

                await
                    _sessionStorage
                        .SetAsync(
                            DomainConstants.RoleKey,
                            role
                        );

                await
                    _sessionStorage
                        .SetAsync(
                            DomainConstants.CurrentLanguageKey,
                            currentLanguage
                        );
            }
            catch (InvalidOperationException ex) when (ex.Message.Contains("prerendering"))
            {
                _logger
                    .LogDebug("Cannot save to session storage during prerendering");
            }

            _logger
                .LogInformation(
                    "User {username} logged in successfully with role {role}",
                    userName,
                    role
                );
        }

        public async Task ClearLocalStateAsync()
        {
            _cachedAuthHeader = null;
            _cachedRefreshToken = null;
            _cachedExpiresAt = null;
            _cachedUsername = null;
            _cachedRole = null;
            _cachedCurrentLanguage = null;

            try
            {
                await _sessionStorage.DeleteAsync(DomainConstants.AuthHeaderKey);
                await _sessionStorage.DeleteAsync(DomainConstants.RefreshTokenKey);
                await _sessionStorage.DeleteAsync(DomainConstants.AccessTokenExpiresAtKey);
                await _sessionStorage.DeleteAsync(DomainConstants.UsernameKey);
                await _sessionStorage.DeleteAsync(DomainConstants.CurrentLanguageKey);
                await _sessionStorage.DeleteAsync(DomainConstants.RoleKey);
            }
            catch (InvalidOperationException ex) when (ex.Message.Contains("prerendering"))
            {
                _logger
                    .LogDebug("Cannot delete from session storage during prerendering");
            }
        }

        public async Task InitializeStoredSessionAsync()
        {
            try
            {
                var authResult =
                    await
                        _sessionStorage
                            .GetAsync<string>(DomainConstants.AuthHeaderKey);

                var usernameResult =
                    await
                        _sessionStorage
                            .GetAsync<string>(DomainConstants.UsernameKey);

                var roleResult =
                    await
                        _sessionStorage
                            .GetAsync<string>(DomainConstants.RoleKey);

                var currentLanguageResult =
                    await
                        _sessionStorage
                            .GetAsync<string>(DomainConstants.CurrentLanguageKey);

                if (authResult.Success && !string.IsNullOrEmpty(authResult.Value))
                {
                    _cachedAuthHeader = authResult.Value;
                    _logger
                        .LogInformation("Auth header restored from session storage");
                }

                if (usernameResult.Success && !string.IsNullOrEmpty(usernameResult.Value))
                {
                    _cachedUsername = usernameResult.Value;
                    _logger
                        .LogInformation("Username restored from session storage");
                }

                if (roleResult.Success && !string.IsNullOrEmpty(roleResult.Value))
                {
                    _cachedRole = roleResult.Value;
                    _logger
                        .LogInformation("Role restored from session storage");
                }

                if (currentLanguageResult.Success && !string.IsNullOrEmpty(currentLanguageResult.Value))
                {
                    _cachedCurrentLanguage = currentLanguageResult.Value;
                    _logger
                        .LogInformation("Current language restored from session storage");
                }

                _initialized = true;
            }
            catch (InvalidOperationException ex) when (ex.Message.Contains("prerendering"))
            {
                // Expected during prerendering - just mark as initialized without restoring
                _logger
                    .LogDebug("Skipping session storage during prerendering");

                _initialized = true;
            }
            catch (Exception ex)
            {
                _logger
                    .LogWarning(
                        ex,
                        "Could not restore auth header from session storage"
                    );

                _initialized = true;
            }
        }
    }
}
