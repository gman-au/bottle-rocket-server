using System.Threading;
using System.Threading.Tasks;
using Rocket.Gcp.Domain;

namespace Rocket.Gcp.Infrastructure
{
    public interface IVisionOcrService
    {
        Task<string> ExtractHandwrittenTextAsync(
            byte[] imageBytes,
            GcpCredential gcpCredential,
            CancellationToken cancellationToken
        );
    }
}