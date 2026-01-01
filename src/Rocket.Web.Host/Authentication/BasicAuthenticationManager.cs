using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Rocket.Domain.Utils;
using Rocket.Interfaces;
using Rocket.Web.Host.Options;

namespace Rocket.Web.Host.Authentication
{
    public class BasicAuthenticationManager : IAuthenticationManager
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<BasicAuthenticationManager> _logger;
        private readonly ProtectedSessionStorage _sessionStorage;
        private string _cachedAuthHeader;
        private Task _initializationTask;
        private bool _initialized = false;

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

        public async Task<bool> LoginAsync(string username, string password)
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

                var authHeader = $"Basic {credentials}";

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
                            .SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    _cachedAuthHeader = authHeader;

                    try
                    {
                        await
                            _sessionStorage
                                .SetAsync(
                                    DomainConstants
                                        .AuthHeaderKey,
                                    authHeader
                                );
                    }
                    catch (InvalidOperationException ex) when (ex.Message.Contains("prerendering"))
                    {
                        _logger
                            .LogDebug("Cannot save to session storage during prerendering");
                    }

                    _logger
                        .LogInformation(
                            "User {username} logged in successfully",
                            username
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

            try
            {
                await 
                    _sessionStorage
                        .DeleteAsync(DomainConstants.AuthHeaderKey);
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
                var result =
                    await
                        _sessionStorage
                            .GetAsync<string>(DomainConstants.AuthHeaderKey);

                if (result.Success && !string.IsNullOrEmpty(result.Value))
                {
                    _cachedAuthHeader =
                        result
                            .Value;

                    _logger
                        .LogInformation("Auth header restored from session storage");
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