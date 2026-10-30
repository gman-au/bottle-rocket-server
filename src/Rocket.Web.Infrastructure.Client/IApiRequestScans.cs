using System.Threading;
using System.Threading.Tasks;
using Rocket.Api.Contracts;
using Rocket.Api.Contracts.Scans;

namespace Rocket.Web.Infrastructure.Client
{
    public interface IApiRequestScans
    {
        Task<MyScansResponse> GetMyScansAsync(
            MyScansRequest request,
            CancellationToken cancellationToken
        );

        Task<ScanSpecifics> GetMyScanAsync(
            string id,
            CancellationToken cancellationToken
        );        
        
        Task<ApiResponse> DeleteScanByIdAsync(
            string id,
            CancellationToken cancellationToken
        );
    }
}