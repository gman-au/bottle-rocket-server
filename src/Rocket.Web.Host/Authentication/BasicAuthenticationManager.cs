using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Rocket.Interfaces;
using Rocket.Web.Host.Options;

namespace Rocket.Web.Host.Authentication
{
    public class BasicAuthenticationManager : IAuthenticationManager
    {
        private readonly ILogger<BasicAuthenticationManager> _logger;
        private readonly HttpClient _httpClient;
        private string _cachedAuthHeader;

        public BasicAuthenticationManager(
            IOptions<ApiConfigurationOptions> apiConfigurationOptionsAccessor,
            ILogger<BasicAuthenticationManager> logger
        )
        {
            _logger = logger;

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

        public bool IsAuthenticated() => !string.IsNullOrEmpty(_cachedAuthHeader);

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

                    _logger
                        .LogInformation(
                            "User {username} logged in successfully",
                            username
                        );

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

        public Task LogoutAsync()
        {
            _cachedAuthHeader = null;

            _logger
                .LogInformation("User logged out");

            return
                Task
                    .CompletedTask;
        }

        public string GetAuthorizationHeader() => _cachedAuthHeader;
    }
}