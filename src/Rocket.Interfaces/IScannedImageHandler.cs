using System.Threading;
using System.Threading.Tasks;

namespace Rocket.Interfaces
{
    public interface IScannedImageHandler
    {
        Task HandleAsync(
            byte[] imageData,
            string contentType,
            string fileExtension,
            string userId,
            CancellationToken cancellationToken
        );
    }
}