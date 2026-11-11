using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Rocket.Domain.Jobs;
using Rocket.Interfaces;

namespace Rocket.Notion.Infrastructure
{
    public interface INotionNoteUploader
    {
        Task UploadTextNoteAsync(
            string integrationSecret,
            string parentNoteId,
            IEnumerable<ArtifactDataElement> elements,
            IJobControl jobControl,
            CancellationToken cancellationToken
        );
        
        Task UploadImageNoteAsync(
            string integrationSecret,
            string parentNoteId,
            string title,
            string fileUploadId,
            CancellationToken cancellationToken
        );
    }
}