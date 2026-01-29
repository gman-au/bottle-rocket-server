using System.Threading;
using System.Threading.Tasks;
using Microsoft.Graph;
using Rocket.Microsofts.Domain;

namespace Rocket.Microsofts.Infrastructure
{
    public interface IGraphClientProvider
    {
        Task<GraphServiceClient> GetClientAsync(
            MicrosoftConnector connector,
            CancellationToken cancellationToken
        );
    }
}