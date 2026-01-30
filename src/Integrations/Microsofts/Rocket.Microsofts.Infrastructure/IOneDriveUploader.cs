using System.Threading;
using System.Threading.Tasks;
using Rocket.Microsofts.Domain;

namespace Rocket.Microsofts.Infrastructure
{
    public interface IOneDriveUploader
    {
        Task UploadFileAsync(
            MicrosoftConnector connector,
            string fileName,
            string folderPath,
            byte[] inputBytes,
            CancellationToken cancellationToken
        );
    }
}