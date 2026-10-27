using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Rocket.Api.Contracts;
using Rocket.Domain.Utils;
using Rocket.Interfaces;
using Rocket.Web.Client.Extensions;
using Rocket.Web.Client.Options;

namespace Rocket.Web.Client.Authentication
{
    public class JwtAuthenticationManager : IAuthenticationManager
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<JwtAuthenticationManager> _logger;
        private readonly ProtectedSessionStorage _sessionStorage;
        private readonly IThemeService _themeService;
        private string _cachedAuthHeader;
        private string _cachedRole;
        private string _cachedUsername;
        private string _cachedCurrentLanguage;
        private Task _initializationTask;
        private bool _initialized;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public JwtAuthenticationManager(
            IOptions<ApiConfigurationOptions> apiConfigurationOptionsAccessor,
            ILogger<JwtAuthenticationManager> logger,
            ProtectedSessionStorage sessionStorage,
            IJsonResolverInstanceProvider jsonResolverInstanceProvider,
            IThemeService themeService
        )
        {
            _logger = logger;
            _sessionStorage = sessionStorage;
            _themeService = themeService;

            var options = apiConfigurationOptionsAccessor.Value;

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

        public async Task SetCurrentLanguageAsync(
            string language
        )
        {
            _cachedCurrentLanguage = language;
        }

        public async Task<bool> LoginAsync(string username, string password, CancellationToken cancellationToken)
        {
            try
            {
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
                                Password = password
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

                    var authHeader = $"{DomainConstants.Bearer} {authResponse.Token}";

                    _cachedAuthHeader = authHeader;
                    _cachedUsername = authResponse.UserName;
                    _cachedRole = authResponse.Role;
                    _cachedCurrentLanguage = authResponse.CurrentLanguage;

                    _themeService
                        .SetDarkMode(authResponse.DarkMode);

                    try
                    {
                        await _sessionStorage.SetAsync(
                            DomainConstants.AuthHeaderKey,
                            authHeader
                        );
                        
                        await _sessionStorage.SetAsync(
                            DomainConstants.UsernameKey,
                            authResponse.UserName
                        );
                        
                        await _sessionStorage.SetAsync(
                            DomainConstants.RoleKey,
                            authResponse.Role
                        );
                        
                        await _sessionStorage.SetAsync(
                            DomainConstants.CurrentLanguageKey,
                            authResponse.CurrentLanguage
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
                            authResponse.UserName,
                            authResponse.Role
                        );

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
            _cachedAuthHeader = null;
            _cachedUsername = null;
            _cachedRole = null;
            _cachedCurrentLanguage = null;

            try
            {
                await _sessionStorage.DeleteAsync(DomainConstants.AuthHeaderKey);
                await _sessionStorage.DeleteAsync(DomainConstants.UsernameKey);
                await _sessionStorage.DeleteAsync(DomainConstants.CurrentLanguageKey);
                await _sessionStorage.DeleteAsync(DomainConstants.RoleKey);
            }
            catch (InvalidOperationException ex) when (ex.Message.Contains("prerendering"))
            {
                _logger
                    .LogDebug("Cannot delete from session storage during prerendering");
            }

            _logger
                .LogInformation("User logged out");

            OnAuthenticationStateChanged?
                .Invoke();
        }

        public async Task<string> GetAuthorizationHeaderAsync()
        {
            await
                EnsureInitializedAsync();

            return
                _cachedAuthHeader;
        }

        public async Task<string> GetUsernameAsync()
        {
            await
                EnsureInitializedAsync();

            return
                _cachedUsername;
        }

        public async Task<string> GetRoleAsync()
        {
            await
                EnsureInitializedAsync();

            return
                _cachedRole;
        }

        public async Task<string> GetCurrentLanguageAsync()
        {
            await
                EnsureInitializedAsync();

            return
                _cachedCurrentLanguage;
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

        public async Task<bool> IsAuthenticatedAsync()
        {
            await
                EnsureInitializedAsync();

            return
                !string
                    .IsNullOrEmpty(_cachedAuthHeader);
        }

        private async Task EnsureInitializedAsync()
        {
            if (_initialized) return;

            // Prevent multiple simultaneous initialization attempts
            if (_initializationTask != null)
            {
                await
                    _initializationTask;

                return;
            }

            _initializationTask = InitializeAsync();

            await
                _initializationTask;
        }

        private async Task InitializeAsync()
        {
            try
            {
                var authResult = await _sessionStorage.GetAsync<string>(DomainConstants.AuthHeaderKey);
                var usernameResult = await _sessionStorage.GetAsync<string>(DomainConstants.UsernameKey);
                var roleResult = await _sessionStorage.GetAsync<string>(DomainConstants.RoleKey);
                var currentLanguageResult = await _sessionStorage.GetAsync<string>(DomainConstants.CurrentLanguageKey);

                if (authResult.Success && !string.IsNullOrEmpty(authResult.Value))
                {
                    _cachedAuthHeader = authResult.Value;
                    _logger.LogInformation("Auth header restored from session storage");
                }

                if (usernameResult.Success && !string.IsNullOrEmpty(usernameResult.Value))
                {
                    _cachedUsername = usernameResult.Value;
                    _logger.LogInformation("Username restored from session storage");
                }

                if (roleResult.Success && !string.IsNullOrEmpty(roleResult.Value))
                {
                    _cachedRole = roleResult.Value;
                    _logger.LogInformation("Role restored from session storage");
                }

                if (currentLanguageResult.Success && !string.IsNullOrEmpty(currentLanguageResult.Value))
                {
                    _cachedCurrentLanguage = currentLanguageResult.Value;
                    _logger.LogInformation("Current language restored from session storage");
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