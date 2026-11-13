using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Rocket.Domain.Jobs;
using Rocket.Interfaces;

namespace Rocket.Local.Infrastructure
{
    public interface IFileWriter
    {
        Task WriteAsync(
            string uploadFilePath,
            IJobControl jobControl,
            IEnumerable<ArtifactDataElement> elements,
            CancellationToken cancellationToken
        );
    }
}