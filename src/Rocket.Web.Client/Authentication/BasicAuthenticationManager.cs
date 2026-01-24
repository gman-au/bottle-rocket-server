using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Rocket.Api.Contracts;
using Rocket.Domain.Core.Utils;
using Rocket.Interfaces;
using Rocket.Web.Client.Extensions;
using Rocket.Web.Client.Options;

namespace Rocket.Web.Client.Authentication
{
    public class BasicAuthenticationManager : IAuthenticationManager
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<BasicAuthenticationManager> _logger;
        private readonly ProtectedSessionStorage _sessionStorage;
        private string _cachedAuthHeader;
        private string _cachedRole;
        private string _cachedUsername;
        private Task _initializationTask;
        private bool _initialized;

        public BasicAuthenticationManager(
            IOptions<ApiConfigurationOptions> apiConfigurationOptionsAccessor,
            ILogger<BasicAuthenticationManager> logger,
            ProtectedSessionStorage sessionStorage
        )
        {
            _logger = logger;
            _sessionStorage = sessionStorage;

            var options = apiConfigurationOptionsAccessor.Value;
            _httpClient = new HttpClient();
            _httpClient.BaseAddress =
                new Uri(
                    options?.BaseUrl ??
                    throw new ConfigurationErrorsException(
                        nameof(options.BaseUrl)
                    )
                );
        }

        public event Action OnAuthenticationStateChanged;

        public async Task<bool> LoginAsync(string username, string password, CancellationToken cancellationToken)
        {
            try
            {
                // Create Basic Auth header
                var credentials =
                    Convert
                        .ToBase64String(
                            Encoding
                                .UTF8
                                .GetBytes($"{username}:{password}")
                        );

                var authHeader = $"{DomainConstants.Basic} {credentials}";

                var request =
                    new HttpRequestMessage(
                        HttpMethod.Post,
                        "/api/connection"
                    );

                request.Headers.Authorization =
                    AuthenticationHeaderValue
                        .Parse(authHeader);

                var response =
                    await
                        _httpClient
                            .SendAsync(
                                request,
                                cancellationToken
                            );

                if (response.IsSuccessStatusCode)
                {
                    var connectionTestResponse =
                        await
                            response
                                .TryParseResponse<ConnectionTestResponse>(
                                    _logger,
                                    cancellationToken
                                );

                    _cachedAuthHeader = authHeader;
                    _cachedUsername = connectionTestResponse.UserName;
                    _cachedRole = connectionTestResponse.Role;

                    try
                    {
                        await _sessionStorage.SetAsync(DomainConstants.AuthHeaderKey, authHeader);
                        await _sessionStorage.SetAsync(DomainConstants.UsernameKey, connectionTestResponse.UserName);
                        await _sessionStorage.SetAsync(DomainConstants.RoleKey, connectionTestResponse.Role);
                    }
                    catch (InvalidOperationException ex) when (ex.Message.Contains("prerendering"))
                    {
                        _logger
                            .LogDebug("Cannot save to session storage during prerendering");
                    }

                    _logger
                        .LogInformation(
                            "User {username} logged in successfully with role {role}",
                            connectionTestResponse.UserName,
                            connectionTestResponse.Role
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

            try
            {
                await _sessionStorage.DeleteAsync(DomainConstants.AuthHeaderKey);
                await _sessionStorage.DeleteAsync(DomainConstants.UsernameKey);
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