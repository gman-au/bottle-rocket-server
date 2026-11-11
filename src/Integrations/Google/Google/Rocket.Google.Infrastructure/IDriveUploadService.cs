using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Rocket.Domain.Jobs;
using Rocket.Google.Domain;
using Rocket.Interfaces;

namespace Rocket.Google.Infrastructure
{
    public interface IDriveUploadService
    {
        Task UploadFileAsync(
            IJobControl jobControl,
            IEnumerable<ArtifactDataElement> elements,
            string targetFolderId,
            GoogleConnector googleConnector,
            CancellationToken cancellationToken
        );
    }
}