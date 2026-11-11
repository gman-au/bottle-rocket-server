using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Rocket.Domain.Jobs;
using Rocket.Interfaces;

namespace Rocket.QuestPdf.Infrastructure
{
    public interface IPdfGenerator
    {
        Task<IEnumerable<ArtifactDataElement>> GeneratePdfFromTextAsync(
            IJobControl jobControl,
            IEnumerable<ArtifactDataElement> elements,
            string artifactFileName,
            bool mergeFiles,
            CancellationToken cancellationToken
        );

        Task<IEnumerable<ArtifactDataElement>> GeneratePdfFromImageAsync(
            IJobControl jobControl,
            IEnumerable<ArtifactDataElement> elements,
            string artifactFileName,
            bool mergeFiles,
            CancellationToken cancellationToken
        );
    }
}