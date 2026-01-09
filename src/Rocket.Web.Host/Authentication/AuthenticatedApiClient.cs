using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Rocket.Domain.Enum;
using Rocket.Domain.Exceptions;
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

        public async Task<HttpResponseMessage> GetAsync(string requestUri, CancellationToken cancellationToken)
        {
            try
            {
                await
                    SetAuthHeaderAsync();

                return
                    await
                        _httpClient
                            .GetAsync(
                                requestUri,
                                cancellationToken
                            );
            }
            catch (Exception ex)
            {
                throw ThrowForConnectivity(ex);
            }
        }

        public async Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content, CancellationToken cancellationToken)
        {
            try
            {
                await
                    SetAuthHeaderAsync();

                return
                    await
                        _httpClient
                            .PostAsync(
                                requestUri,
                                content,
                                cancellationToken
                            );
            }
            catch (Exception ex)
            {
                throw ThrowForConnectivity(ex);
            }
        }

        public async Task<HttpResponseMessage> PostAsJsonAsync<T>(string requestUri, T content, CancellationToken cancellationToken)
        {
            try
            {
                await
                    SetAuthHeaderAsync();

                return
                    await
                        _httpClient
                            .PostAsJsonAsync(
                                requestUri,
                                content,
                                cancellationToken
                            );
            }
            catch (Exception ex)
            {
                throw ThrowForConnectivity(ex);
            }
        }

        public async Task<HttpResponseMessage> PutAsync(string requestUri, HttpContent content, CancellationToken cancellationToken)
        {
            try
            {
                await
                    SetAuthHeaderAsync();

                return
                    await
                        _httpClient
                            .PutAsync(
                                requestUri,
                                content,
                                cancellationToken
                            );
            }
            catch (Exception ex)
            {
                throw ThrowForConnectivity(ex);
            }
        }

        public async Task<HttpResponseMessage> DeleteAsync(string requestUri, CancellationToken cancellationToken)
        {
            try
            {
                await
                    SetAuthHeaderAsync();

                return
                    await
                        _httpClient
                            .DeleteAsync(
                                requestUri,
                                cancellationToken
                            );
            }
            catch (Exception ex)
            {
                throw ThrowForConnectivity(ex);
            }
        }

        private async Task SetAuthHeaderAsync()
        {
            var authHeader =
                await
                    _authenticationManager
                        .GetAuthorizationHeaderAsync();

            if (!string.IsNullOrEmpty(authHeader))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    AuthenticationHeaderValue
                        .Parse(authHeader);
            }
        }

        private static Exception ThrowForConnectivity(Exception ex)
        {
            if (ex is not HttpRequestException hex) return ex;

            if (hex.Message.Contains("No connection could be made"))
            {
                return new RocketException(
                    "Could not connect to server.",
                    ApiStatusCodeEnum.ServerConnectionError
                );
            }

            return ex;
        }
    }
}