using System.Threading;
using System.Threading.Tasks;

namespace Rocket.Interfaces
{
    public interface IThumbnailer
    {
        Task<string> GenerateBase64ThumbnailAsync(
            byte[] imageBytes,
            CancellationToken cancellationToken
        );
    }
}