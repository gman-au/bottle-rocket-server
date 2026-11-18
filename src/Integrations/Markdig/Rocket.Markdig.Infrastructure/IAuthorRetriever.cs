using System.Threading;
using System.Threading.Tasks;

namespace Rocket.Markdig.Infrastructure
{
    public interface IAuthorRetriever
    {
        Task<string> RetrieveUserNameAsync(
            string userId,
            CancellationToken cancellationToken
        );
    }
}