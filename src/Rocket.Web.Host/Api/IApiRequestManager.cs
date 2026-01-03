using System.Threading;
using System.Threading.Tasks;
using Rocket.Api.Contracts;

namespace Rocket.Web.Host.Api
{
    public interface IApiRequestManager
    {
        Task<MyScansResponse> GetMyScansAsync(
            MyScansRequest request,
            CancellationToken cancellationToken
        );
        
        Task<MyScanItemDetail> GetMyScanAsync(
            string id,
            CancellationToken cancellationToken
        );
    }
}