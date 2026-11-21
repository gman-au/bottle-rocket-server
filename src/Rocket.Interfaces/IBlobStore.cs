using System;
using System.Threading;
using System.Threading.Tasks;

namespace Rocket.Interfaces
{
    public interface IBlobStore
    {
        Task<string> SaveImageAsync(
            string userId,
            byte[] image,
            string fileExtension,
            CancellationToken cancellationToken
        );

        Task<byte[]> LoadImageAsync(
            string userId,
            string filePath,
            string fileExtension,
            CancellationToken cancellationToken
        );

        Task<bool> DeleteImageAsync(
            string userId,
            string blobId,
            string fileExtension,
            CancellationToken cancellationToken
        );

        Task<Tuple<long, long>> GetDriveInfoAsync(
            string userId,
            CancellationToken cancellationToken
        );
    }
}