using System.Threading;
using System.Threading.Tasks;
using Rocket.Domain.Jobs;
using Rocket.Gcp.Domain;
using Rocket.Interfaces;

namespace Rocket.Gcp.Infrastructure
{
    public interface IVisionOcrService
    {
        Task<ExecutionStepArtifact> ExtractHandwrittenTextAsync(
            IJobControl jobControl,
            ExecutionStepArtifact artifact,
            GcpCredential gcpCredential,
            int timeoutInMinutes,
            CancellationToken cancellationToken
        );
    }
}