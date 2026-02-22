using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Rocket.Google.Contracts;
using Rocket.Google.Domain;

namespace Rocket.Google.Infrastructure
{
    public interface IDriveFolderSearcher
    {
        Task<IEnumerable<GoogleDriveFolder>> GetFoldersAsync(
            GoogleConnector googleConnector,
            CancellationToken cancellationToken
        );
    }
}