using System.Threading;
using System.Threading.Tasks;
using Rocket.Gcp.Domain;

namespace Rocket.Gcp.Infrastructure
{
    public class DriveService : IDriveService
    {
        public Task UploadFileAsync(
            byte[] fileBytes,
            string fileExtension,
            GcpCredential gcpCredential,
            CancellationToken cancellationToken
        )
        {
            throw new System.NotImplementedException();
        }
    }
}