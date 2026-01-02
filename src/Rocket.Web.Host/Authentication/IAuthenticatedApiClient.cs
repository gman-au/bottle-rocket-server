using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Rocket.Web.Host.Authentication
{
    public interface IAuthenticatedApiClient
    {
        Task<HttpResponseMessage> GetAsync(string requestUri, CancellationToken cancellationToken);
        
        Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content, CancellationToken cancellationToken);
        
        Task<HttpResponseMessage> PostAsJsonAsync<T>(string requestUri, T content, CancellationToken cancellationToken);
        
        Task<HttpResponseMessage> PutAsync(string requestUri, HttpContent content, CancellationToken cancellationToken);
        
        Task<HttpResponseMessage> DeleteAsync(string requestUri, CancellationToken cancellationToken);
    }
}