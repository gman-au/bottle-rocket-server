using System.Threading;
using System.Threading.Tasks;

namespace Rocket.Interfaces
{
    public interface IScannedImageHandler
    {
        Task HandleAsync(
            byte[] toArray,
            CancellationToken cancellationToken
        );
    }
}