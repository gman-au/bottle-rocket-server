using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Rocket.Domain.Jobs;
using Rocket.Interfaces;
using Rocket.Microsofts.Domain;

namespace Rocket.Microsofts.Infrastructure
{
    public interface IOneDriveUploader
    {
        Task UploadFileAsync(
            MicrosoftConnector connector,
            IJobControl jobControl,
            IEnumerable<ArtifactDataElement> elements,
            string folderPath,
            CancellationToken cancellationToken
        );
    }
}