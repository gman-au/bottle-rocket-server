using System.Threading;
using System.Threading.Tasks;
using Rocket.Api.Contracts.Users;

namespace Rocket.Web.Client
{
    public interface IApiRequestUsers
    {
        Task<UserSpecifics> GetUserByIdAsync(
            string id,
            CancellationToken cancellationToken
        );

        Task<UpdateUserResponse> UpdateUserAsync(
            UserSpecifics user,
            CancellationToken cancellationToken
        );

        Task<UpdateUserResponse> UpdateDarkModeAsync(
            SetUserDarkModeRequest request,
            CancellationToken cancellationToken
        );

        Task<CreateUserResponse> CreateUserAsync(
            CreateUserRequest user,
            CancellationToken cancellationToken
        );

        Task<FetchUsersResponse> GetUsersAsync(
            FetchUsersRequest request,
            CancellationToken cancellationToken
        );
    }
}