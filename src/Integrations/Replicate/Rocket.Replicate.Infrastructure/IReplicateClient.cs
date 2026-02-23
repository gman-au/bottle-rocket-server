using System.Threading;
using System.Threading.Tasks;
using Rocket.Replicate.Infrastructure.Definition;

namespace Rocket.Replicate.Infrastructure
{
    public interface IReplicateClient
    {
        Task<(string, string)> UploadFileAsync(
            string apiToken,
            byte[] bytes,
            string fileName,
            string fileExtension,
            CancellationToken cancellationToken
        );

        Task<string> CreatePredictionAsync<T>(
            string apiToken,
            string version,
            T input,
            CancellationToken cancellationToken,
            string customEndpoint = null
        ) where T : IReplicateInput;

        Task<ReplicatePredictionResponse<T>> GetPredictionStatusAsync<T>(
            string apiToken,
            string predictionId,
            CancellationToken cancellationToken
        ) where T : IReplicateOutput;

        Task<T> WaitUntilPredictionCompletesAsync<T>(
            string apiToken,
            string predictionId,
            CancellationToken cancellationToken,
            int timeoutInSeconds = 300
        ) where T : IReplicateOutput;

        Task DeleteUploadAsync(string apiToken, string fileId);
    }
}