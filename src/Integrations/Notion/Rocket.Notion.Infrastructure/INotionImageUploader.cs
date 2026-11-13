using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Rocket.Domain.Jobs;
using Rocket.Interfaces;

namespace Rocket.Notion.Infrastructure
{
    public interface INotionImageUploader
    {
        Task UploadImageNoteAsync(
            string integrationSecret,
            string parentNoteId,
            IEnumerable<ArtifactDataElement> elements,
            IJobControl jobControl,
            CancellationToken cancellationToken
        );
    }
}