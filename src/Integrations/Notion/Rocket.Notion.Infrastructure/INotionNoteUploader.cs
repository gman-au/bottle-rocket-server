using System.Threading;
using System.Threading.Tasks;

namespace Rocket.Notion.Infrastructure
{
    public interface INotionNoteUploader
    {
        Task UploadTextNoteAsync(
            string integrationSecret,
            string parentNoteId,
            string title,
            string textContent,
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