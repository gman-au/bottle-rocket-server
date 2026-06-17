using System.Threading;
using System.Threading.Tasks;
using Rocket.Google.Domain;

namespace Rocket.Google.Infrastructure
{
    public interface IDriveUploadService
    {
        Task UploadFileAsync(
            byte[] fileBytes,
            string fileName,
            string fileExtension,
            string targetFolderId,
            GoogleConnector googleConnector,
            CancellationToken cancellationToken
        );
    }
}