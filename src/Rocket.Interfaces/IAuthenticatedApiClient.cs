using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Rocket.Interfaces
{
    public interface IAuthenticatedApiClient
    {
        Task<HttpResponseMessage> GetAsync(string requestUri, CancellationToken cancellationToken);
        
        Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content, CancellationToken cancellationToken);
        
        Task<HttpResponseMessage> PostAsJsonAsync<T>(string requestUri, T content, CancellationToken cancellationToken);
        
        Task<HttpResponseMessage> PutAsync(string requestUri, HttpContent content, CancellationToken cancellationToken);
        
        Task<HttpResponseMessage> DeleteAsync(string requestUri, CancellationToken cancellationToken);
        
        Task<HttpResponseMessage> PatchAsync(string requestUri, HttpContent content, CancellationToken cancellationToken);
        
        Task<HttpResponseMessage> PatchAsJsonAsync<T>(string requestUri, T content, CancellationToken cancellationToken);
    }
}