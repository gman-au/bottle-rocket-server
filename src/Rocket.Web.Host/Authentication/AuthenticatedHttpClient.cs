using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Rocket.Interfaces;
using Rocket.Web.Host.Options;

namespace Rocket.Web.Host.Authentication
{
    public class AuthenticatedApiClient : IAuthenticatedApiClient
    {
        private readonly IAuthenticationManager _authenticationManager;
        private readonly HttpClient _httpClient;

        public AuthenticatedApiClient(
            IOptions<ApiConfigurationOptions> apiConfigurationOptionsAccessor,
            IAuthenticationManager authenticationManager
        )
        {
            _authenticationManager = authenticationManager;
            
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

        public Task<HttpResponseMessage> GetAsync(string requestUri, CancellationToken cancellationToken)
        {
            SetAuthHeader();

            return
                _httpClient
                    .GetAsync(
                        requestUri,
                        cancellationToken
                    );
        }

        public Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content, CancellationToken cancellationToken)
        {
            SetAuthHeader();

            return
                _httpClient
                    .PostAsync(
                        requestUri,
                        content,
                        cancellationToken
                    );
        }

        public Task<HttpResponseMessage> PutAsync(string requestUri, HttpContent content, CancellationToken cancellationToken)
        {
            SetAuthHeader();

            return
                _httpClient
                    .PutAsync(
                        requestUri,
                        content,
                        cancellationToken
                    );
        }

        public Task<HttpResponseMessage> DeleteAsync(string requestUri, CancellationToken cancellationToken)
        {
            SetAuthHeader();

            return
                _httpClient
                    .DeleteAsync(
                        requestUri,
                        cancellationToken
                    );
        }

        private void SetAuthHeader()
        {
            var authHeader =
                _authenticationManager
                    .GetAuthorizationHeader();

            if (!string.IsNullOrEmpty(authHeader))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    AuthenticationHeaderValue
                        .Parse(authHeader);
            }
        }
    }
}