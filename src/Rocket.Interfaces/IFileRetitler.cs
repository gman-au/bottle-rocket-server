using System.Threading;
using System.Threading.Tasks;

namespace Rocket.Interfaces
{
    public interface IFileRetitler
    {
        Task<string> RetitleAsync(
            string rawTextData,
            string scanId,
            string userId,
            CancellationToken cancellationToken
        );
    }
}