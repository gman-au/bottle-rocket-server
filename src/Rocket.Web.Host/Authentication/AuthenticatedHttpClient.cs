using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Rocket.Interfaces;

namespace Rocket.Web.Host.Authentication
{
    public class AuthenticatedApiClient(
        HttpClient httpClient,
        IAuthenticationManager authenticationManager
    ) : IAuthenticatedApiClient
    {
        public Task<HttpResponseMessage> GetAsync(string requestUri, CancellationToken cancellationToken)
        {
            SetAuthHeader();

            return
                httpClient
                    .GetAsync(
                        requestUri,
                        cancellationToken
                    );
        }

        public Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content, CancellationToken cancellationToken)
        {
            SetAuthHeader();

            return
                httpClient
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
                httpClient
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
                httpClient
                    .DeleteAsync(
                        requestUri,
                        cancellationToken
                    );
        }

        private void SetAuthHeader()
        {
            var authHeader =
                authenticationManager
                    .GetAuthorizationHeader();

            if (!string.IsNullOrEmpty(authHeader))
            {
                httpClient.DefaultRequestHeaders.Authorization =
                    AuthenticationHeaderValue
                        .Parse(authHeader);
            }
        }
    }
}