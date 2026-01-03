using System.Threading;
using System.Threading.Tasks;

namespace Rocket.Interfaces
{
    public interface IBlobStore
    {
        Task<string> SaveImageAsync(
            byte[] image,
            string fileExtension,
            CancellationToken cancellationToken
        );

        Task<byte[]> LoadImageAsync(
            string filePath,
            string fileExtension,
            CancellationToken cancellationToken
        );
    }
}