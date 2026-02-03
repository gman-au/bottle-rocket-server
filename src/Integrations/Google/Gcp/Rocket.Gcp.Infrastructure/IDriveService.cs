using System.Threading;
using System.Threading.Tasks;
using Rocket.Gcp.Domain;

namespace Rocket.Gcp.Infrastructure
{
    public interface IDriveService
    {
        Task UploadFileAsync(
            byte[] fileBytes,
            string fileExtension,
            GcpCredential gcpCredential,
            CancellationToken cancellationToken
        );
    }
}