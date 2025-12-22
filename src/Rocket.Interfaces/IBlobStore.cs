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
        
        Task<byte[]> GetImageAsync(string id, CancellationToken cancellationToken);
    }
}