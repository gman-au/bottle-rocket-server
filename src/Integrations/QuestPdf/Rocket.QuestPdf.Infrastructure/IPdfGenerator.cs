using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Rocket.Domain.Jobs;
using Rocket.Interfaces;

namespace Rocket.QuestPdf.Infrastructure
{
    public interface IPdfGenerator
    {
        Task<IEnumerable<(string, byte[])>> GeneratePdfFromTextAsync(
            IJobControl jobControl,
            IEnumerable<ArtifactDataElement> elements,
            bool mergeFiles,
            CancellationToken cancellationToken
        );

        Task<IEnumerable<(string, byte[])>> GeneratePdfFromImageAsync(
            IJobControl jobControl,
            IEnumerable<ArtifactDataElement> elements,
            bool mergeFiles,
            CancellationToken cancellationToken
        );
    }
}