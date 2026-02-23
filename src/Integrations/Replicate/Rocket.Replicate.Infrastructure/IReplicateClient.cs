using System.Threading;
using System.Threading.Tasks;
using Rocket.Replicate.Infrastructure.Definition;

namespace Rocket.Replicate.Infrastructure
{
    public interface IReplicateClient
    {
        Task<string> UploadFileAsync(
            string apiToken,
            byte[] bytes,
            string fileName,
            CancellationToken cancellationToken
        );

        Task<string> CreatePredictionAsync<T>(
            string apiToken,
            string version,
            T input,
            CancellationToken cancellationToken
        ) where T : IReplicateInput;

        Task<ReplicatePredictionResponse<T>> GetPredictionStatusAsync<T>(
            string apiToken,
            string predictionId,
            CancellationToken cancellationToken
        );
    }
}