using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Rocket.Domain.Jobs;
using Rocket.Interfaces;

namespace Rocket.Markdig.Infrastructure
{
    public interface IEpubGenerator
    {
        Task<IEnumerable<ArtifactDataElement>> GenerateEpubFromTextAsync(
            IJobControl jobControl,
            ExecutionStepArtifact artifact,
            bool mergeFiles,
            string author,
            CancellationToken cancellationToken
        );
    }
}