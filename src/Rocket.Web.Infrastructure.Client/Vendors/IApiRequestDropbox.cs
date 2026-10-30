using System.Threading;
using System.Threading.Tasks;
using Rocket.Api.Contracts;
using Rocket.Dropbox.Contracts;

namespace Rocket.Web.Infrastructure.Client.Vendors
{
    public interface IApiRequestDropbox
    {
        Task<ApiResponse> FinalizeDropboxConnectorAsync(
            FinalizeDropboxConnectorRequest request,
            CancellationToken cancellationToken
        );
    }
}