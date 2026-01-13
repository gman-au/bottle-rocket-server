using System.Threading;
using System.Threading.Tasks;
using Rocket.Api.Contracts;
using Rocket.Api.Contracts.Connectors;
using Rocket.Integrations.Dropbox.Contracts;

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

        Task<UserDetail> GetUserByIdAsync(
            string id,
            CancellationToken cancellationToken
        );

        Task<UpdateUserResponse> UpdateUserAsync(
            UserDetail user,
            CancellationToken cancellationToken
        );

        Task<CreateUserResponse> CreateUserAsync(
            CreateUserRequest user,
            CancellationToken cancellationToken
        );

        Task<StartupPhaseResponse> GetStartupPhaseAsync(CancellationToken cancellationToken);

        Task<FetchUsersResponse> GetUsersAsync(
            FetchUsersRequest request,
            CancellationToken cancellationToken
        );

        Task<FetchConnectorsResponse> GetMyConnectorsAsync(
            FetchConnectorsRequest request,
            CancellationToken baseCancellationToken
        );

        Task<CreateDropboxConnectorResponse> CreateDropboxConnectorAsync(
            CreateDropboxConnectorRequest connector,
            CancellationToken cancellationToken
        );

        public Task<ApiResponse> UpdateDropboxConnectorAsync(
            FinalizeDropboxConnectorRequest request,
            CancellationToken cancellationToken
        );

        Task<ApiResponse> DeleteDropboxConnectorByIdAsync(
            string id,
            CancellationToken cancellationToken
        );
    }
}