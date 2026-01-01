using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Rocket.Interfaces;

namespace Rocket.Web.Host.Authentication
{
    public class BasicAuthenticationManager(
        HttpClient httpClient,
        ILogger<BasicAuthenticationManager> logger
    ) : IAuthenticationManager
    {
        private string _cachedAuthHeader;

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
                        httpClient
                            .SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    _cachedAuthHeader = authHeader;
                    
                    logger
                        .LogInformation(
                            "User {username} logged in successfully",
                            username
                        );

                    return true;
                }

                logger
                    .LogWarning(
                        "Login failed for user {username}: {statusCode}",
                        username,
                        response.StatusCode
                    );

                return false;
            }
            catch (Exception ex)
            {
                logger
                    .LogError(
                        ex,
                        "Error during login for user {username}",
                        username
                    );

                return false;
            }
        }

        public Task LogoutAsync()
        {
            _cachedAuthHeader = null;
            
            logger
                .LogInformation("User logged out");
            
            return 
                Task
                    .CompletedTask;
        }

        public string GetAuthorizationHeader() => _cachedAuthHeader;
    }
}