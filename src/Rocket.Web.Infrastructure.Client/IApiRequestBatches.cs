using System.Threading;
using System.Threading.Tasks;
using Rocket.Api.Contracts;
using Rocket.Api.Contracts.Batches;

namespace Rocket.Web.Infrastructure.Client
{
    public interface IApiRequestBatches
    {
        Task<MyBatchesResponse> GetMyBatchesAsync(
            MyBatchesRequest request,
            CancellationToken cancellationToken
        );

        Task<BatchSpecifics> GetMyBatchAsync(
            string id,
            CancellationToken cancellationToken
        );        
        
        Task<ApiResponse> DeleteBatchByIdAsync(
            string id,
            CancellationToken cancellationToken
        );
    }
}