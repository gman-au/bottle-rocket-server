using System.Threading;
using System.Threading.Tasks;
using Rocket.Api.Contracts;
using Rocket.Google.Contracts;

namespace Rocket.Web.Infrastructure.Client.Vendors
{
    public interface IApiRequestGoogle
    {
        Task<GoogleAuthInitiateResponse> InitiateGoogleConnectorAuthAsync(
            GoogleAuthInitiateRequest request,
            CancellationToken cancellationToken
        );

        Task<ApiResponse> FinalizeGoogleConnectorAsync(
            GoogleAuthFinalizeRequest request,
            CancellationToken cancellationToken
        );

        Task<GetGoogleDriveFoldersResponse> GetGoogleFoldersAsync(
            GetGoogleDriveFoldersRequest request,
            CancellationToken cancellationToken
        );
    }
}