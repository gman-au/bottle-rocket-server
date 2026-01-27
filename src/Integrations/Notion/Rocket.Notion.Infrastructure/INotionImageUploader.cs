using System.Threading;
using System.Threading.Tasks;

namespace Rocket.Notion.Infrastructure
{
    public interface INotionImageUploader
    {
        Task UploadImageNoteAsync(
            string integrationSecret,
            string parentNoteId,
            string title,
            byte[] imageBytes,
            string fileExtension,
            CancellationToken cancellationToken
        );
    }
}