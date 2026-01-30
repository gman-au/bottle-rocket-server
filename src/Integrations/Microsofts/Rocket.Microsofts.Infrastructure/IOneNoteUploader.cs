using System.Threading;
using System.Threading.Tasks;
using Rocket.Microsofts.Domain;

namespace Rocket.Microsofts.Infrastructure
{
    public interface IOneNoteUploader
    {
        Task UploadTextNoteAsync(
            MicrosoftConnector connector,
            string sectionId,
            string pageTitle,
            string textContent,
            CancellationToken cancellationToken
        );
        
        Task UploadImageNoteAsync(
            MicrosoftConnector connector,
            string sectionId,
            string fileExtension,
            string pageTitle,
            byte[] imageBytes,
            CancellationToken cancellationToken
        );
    }
}