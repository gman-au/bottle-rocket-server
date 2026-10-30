using System;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Rocket.Api.Contracts.Auth;
using Rocket.Domain.Utils;
using Rocket.Interfaces;
using Rocket.Web.Infrastructure.Client;
using Rocket.Web.Infrastructure.Client.Extensions;
using Rocket.Web.Infrastructure.Client.Options;

namespace Rocket.Web.Infrastructure.Auth
{
    public class JwtAuthenticationManager : IAuthenticationManager
    {
        private static readonly TimeSpan RefreshSkew = TimeSpan.FromSeconds(60);

        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonSerializerOptions;
        private readonly ILogger<JwtAuthenticationManager> _logger;

        private readonly SemaphoreSlim _refreshLock = new(
            1,
            1
        );

        private readonly ISessionRetention _sessionRetention;
        private readonly IThemeService _themeService;

        private Task _initializationTask;

        public JwtAuthenticationManager(
            IOptions<ApiConfigurationOptions> apiConfigurationOptionsAccessor,
            ILogger<JwtAuthenticationManager> logger,
            IJsonResolverInstanceProvider jsonResolverInstanceProvider,
            ISessionRetention sessionRetention,
            IThemeService themeService
        )
        {
            _logger = logger;
            _sessionRetention = sessionRetention;
            _themeService = themeService;

            var options =
                apiConfigurationOptionsAccessor
                    .Value;

            _httpClient = new HttpClient();
            
            _httpClient.BaseAddress =
                new Uri(
                    options?.BaseUrl ??
                    throw new ConfigurationErrorsException(
                        nameof(options.BaseUrl)
                    )
                );

            _jsonSerializerOptions =
                jsonResolverInstanceProvider
                    .GetSerializationOptions();
        }

        public event Action OnAuthenticationStateChanged;

        public async Task<bool> LoginAsync(
            string username,
            string password,
            CancellationToken cancellationToken
        )
        {
            try
            {
                //TODO
                var deviceName = "Toast"; //BuildDeviceName();

                var request =
                    new HttpRequestMessage(
                        HttpMethod.Post,
                        "/api/auth/login"
                    );

                request.Content =
                    JsonContent
                        .Create(
                            new AccountLoginRequest
                            {
                                Username = username,
                                Password = password,
                                DeviceName = deviceName,
                                UseCookie = false
                            }
                        );

                var response =
                    await
                        _httpClient
                            .SendAsync(
                                request,
                                cancellationToken
                            );

                if (response.IsSuccessStatusCode)
                {
                    var authResponse =
                        await
                            response
                                .TryParseResponse<AccountLoginResponse>(
                                    _jsonSerializerOptions,
                                    _logger,
                                    cancellationToken
                                );

                    await
                        _sessionRetention
                            .RetainSessionUserAsync(
                                authResponse.UserName,
                                authResponse.Role,
                                authResponse.CurrentLanguage,
                                cancellationToken
                            );

                    await
                        _sessionRetention
                            .RetainSessionTokensAsync(
                                authResponse.AccessToken,
                                authResponse.RefreshToken,
                                authResponse.AccessTokenExpiresAt,
                                cancellationToken
                            );

                    _themeService
                        .SetDarkMode(authResponse.DarkMode);

                    OnAuthenticationStateChanged?
                        .Invoke();

                    return true;
                }

                _logger
                    .LogWarning(
                        "Login failed for user {username}: {statusCode}",
                        username,
                        response.StatusCode
                    );

                return false;
            }
            catch (Exception ex)
            {
                _logger
                    .LogError(
                        ex,
                        "Error during login for user {username}",
                        username
                    );

                throw;
            }
        }

        public async Task LogoutAsync()
        {
            try
            {
                // Goes through the getter so an about-to-expire access token is refreshed first.
                // An expired token would get a 401 and the session would never be revoked.
                var header =
                    await
                        GetAuthorizationHeaderAsync();

                if (!string.IsNullOrEmpty(header))
                {
                    using var request =
                        new HttpRequestMessage(
                            HttpMethod.Post,
                            "/api/auth/logout"
                        );

                    request
                        .Headers
                        .TryAddWithoutValidation(
                            "Authorization",
                            header
                        );

                    await
                        _httpClient
                            .SendAsync(request);
                }
            }
            catch (Exception ex)
            {
                _logger
                    .LogWarning(
                        ex,
                        "Server-side logout failed; clearing local session anyway"
                    );
            }

            await
                ClearLocalStateAsync();

            _logger
                .LogInformation("User logged out");

            OnAuthenticationStateChanged?
                .Invoke();
        }

        public async Task<string> GetAuthorizationHeaderAsync()
        {
            await
                EnsureInitializedAsync();

            if (NeedsRefresh())
                await
                    RefreshAsync();

            return
                _sessionRetention
                    .CachedAuthHeader;
        }

        public async Task<bool> IsAuthenticatedAsync()
        {
            await
                EnsureInitializedAsync();

            return
                !string
                    .IsNullOrEmpty(_sessionRetention.CachedAuthHeader);
        }

        public async Task<string> GetUsernameAsync()
        {
            await
                EnsureInitializedAsync();

            return
                _sessionRetention
                    .CachedUserName;
        }

        public async Task<string> GetRoleAsync()
        {
            await
                EnsureInitializedAsync();

            return
                _sessionRetention
                    .CachedRole;
        }

        public async Task<string> GetCurrentLanguageAsync()
        {
            await
                EnsureInitializedAsync();

            return
                _sessionRetention
                    .CachedCurrentLanguage;
        }

        public async Task<bool> IsRootAdminAsync()
        {
            var username =
                await
                    GetUsernameAsync();

            return
                username?
                    .Equals(
                        DomainConstants.RootAdminUserName,
                        StringComparison.OrdinalIgnoreCase
                    )
                ?? false;
        }

        public async Task<bool> RefreshAsync(
            CancellationToken cancellationToken = default
        )
        {
            await
                EnsureInitializedAsync();

            var tokenUsed = _sessionRetention.CachedRefreshToken;

            if (string.IsNullOrEmpty(tokenUsed)) return false;

            await
                _refreshLock
                    .WaitAsync(cancellationToken);

            try
            {
                // Someone else refreshed while we waited. Using the old token again
                // would trip replay detection, so just use their result.
                if (!string.Equals(
                        tokenUsed,
                        _sessionRetention.CachedRefreshToken
                    ))
                    return !string.IsNullOrEmpty(_sessionRetention.CachedRefreshToken);

                HttpResponseMessage response;

                try
                {
                    using var request = new HttpRequestMessage(
                        HttpMethod.Post,
                        "/api/auth/refresh"
                    );

                    request.Content =
                        JsonContent
                            .Create(
                                new RefreshRequest
                                {
                                    RefreshToken = tokenUsed
                                }
                            );

                    response =
                        await
                            _httpClient
                                .SendAsync(
                                    request,
                                    cancellationToken
                                );
                }
                catch (HttpRequestException ex)
                {
                    // API unreachable: keep the session, the next call can retry
                    _logger
                        .LogWarning(
                            ex,
                            "Token refresh failed: API unreachable"
                        );
                    return false;
                }

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    // Session revoked or expired: this is a real logout
                    _logger
                        .LogInformation("Refresh token rejected, clearing local session");

                    await
                        ClearLocalStateAsync();

                    return false;
                }

                if (!response.IsSuccessStatusCode)
                {
                    _logger
                        .LogWarning(
                            "Token refresh failed: {statusCode}",
                            response.StatusCode
                        );
                    return false; // 5xx etc. is not a logout
                }

                var authResponse =
                    await
                        response
                            .TryParseResponse<AccountLoginResponse>(
                                _jsonSerializerOptions,
                                _logger,
                                cancellationToken
                            );

                await
                    _sessionRetention
                        .RetainSessionTokensAsync(
                            authResponse.AccessToken,
                            authResponse.RefreshToken,
                            authResponse.AccessTokenExpiresAt,
                            cancellationToken
                        );

                return true;
            }
            finally
            {
                _refreshLock
                    .Release();
            }
        }

        private async Task EnsureInitializedAsync()
        {
            if (_sessionRetention.IsInitialized()) return;

            // Prevent multiple simultaneous initialization attempts
            if (_initializationTask != null)
            {
                await
                    _initializationTask;

                return;
            }

            _initializationTask =
                _sessionRetention
                    .InitializeStoredSessionAsync();

            await
                _initializationTask;
        }


        private bool NeedsRefresh() =>
            !string.IsNullOrEmpty(_sessionRetention.CachedRefreshToken)
            && _sessionRetention.CachedExpiresAt is { } exp
            && exp - DateTime.UtcNow < RefreshSkew;

        private async Task ClearLocalStateAsync()
        {
            await
                _sessionRetention
                    .ClearLocalStateAsync();

            OnAuthenticationStateChanged?
                .Invoke();
        }
    }
}