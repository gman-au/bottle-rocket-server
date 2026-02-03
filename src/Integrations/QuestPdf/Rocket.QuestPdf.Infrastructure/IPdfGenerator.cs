using System.Threading;
using System.Threading.Tasks;

namespace Rocket.QuestPdf.Infrastructure
{
    public interface IPdfGenerator
    {
        Task<byte[]> GeneratePdfFromTextAsync(
            string rawText,
            CancellationToken cancellationToken
        );

        Task<byte[]> GeneratePdfFromImageAsync(
            byte[] imageBytes,
            CancellationToken cancellationToken
        );
    }
}