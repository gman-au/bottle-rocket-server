using System.Threading;
using System.Threading.Tasks;
using Rocket.Api.Contracts;
using Rocket.Api.Contracts.Scans;
using Rocket.Api.Contracts.Transcriptions;

namespace Rocket.Web.Infrastructure.Client
{
    public interface IApiRequestScans
    {
        Task<ScanSpecifics> GetMyScanAsync(
            string batchId,
            string scanId,
            CancellationToken cancellationToken
        );        
        
        Task<ApiResponse> DeleteScanByIdAsync(
            string batchId,
            string scanId,
            CancellationToken cancellationToken
        );

        Task<GetTranscriptionResponse> GetMyTranscriptionAsync(
            string batchId,
            string scanId,
            CancellationToken cancellationToken
        );
        
        Task<ApiResponse> DeleteTranscriptionByIdAsync(
            string batchId,
            string scanId,
            CancellationToken cancellationToken
        );
    }
}