using System.Threading;
using System.Threading.Tasks;
using Rocket.Microsofts.Contracts;

namespace Rocket.Web.Client.Vendors
{
    public interface IApiRequestMicrosoft
    {
        Task<MicrosoftAuthInitiateResponse> InitiateMicrosoftConnectorAuthAsync(
            MicrosoftAuthInitiateRequest request,
            CancellationToken cancellationToken
        );

        Task<GetOneNoteSectionsResponse> GetOneNoteSectionsAsync(
            GetOneNoteSectionsRequest request,
            CancellationToken cancellationToken
        );
    }
}