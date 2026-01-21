using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Rocket.Api.Contracts.Users;
using Rocket.Web.Client.Extensions;

namespace Rocket.Web.Client
{
    public partial class ApiRequestManager
    {
        public async Task<FetchUsersResponse> GetUsersAsync(
            FetchUsersRequest request,
            CancellationToken cancellationToken
        )
        {
            logger
                .LogInformation("Received get users request");

            var response =
                await
                    authenticatedApiClient
                        .PostAsJsonAsync(
                            "/api/users/fetch",
                            request,
                            cancellationToken
                        );

            var result =
                await
                    response
                        .TryParseResponse<FetchUsersResponse>(
                            logger,
                            cancellationToken
                        );

            EnsureApiSuccessStatusCode(result);
            EnsureHttpSuccessStatusCode(response);

            return result;
        }

        public async Task<UserSpecifics> GetUserByIdAsync(
            string id,
            CancellationToken cancellationToken
        )
        {
            logger
                .LogInformation("Received Get User request");

            var response =
                await
                    authenticatedApiClient
                        .GetAsync(
                            $"/api/users/get/{id}",
                            cancellationToken
                        );

            var result =
                await
                    response
                        .TryParseResponse<UserSpecifics>(
                            logger,
                            cancellationToken
                        );

            EnsureApiSuccessStatusCode(result);
            EnsureHttpSuccessStatusCode(response);

            return result;
        }

        public async Task<UpdateUserResponse> UpdateUserAsync(
            UserSpecifics user,
            CancellationToken cancellationToken
        )
        {
            logger
                .LogInformation("Received Update User request");

            var response =
                await
                    authenticatedApiClient
                        .PostAsJsonAsync(
                            "/api/users/update",
                            user,
                            cancellationToken
                        );

            var result =
                await
                    response
                        .TryParseResponse<UpdateUserResponse>(
                            logger,
                            cancellationToken
                        );

            EnsureApiSuccessStatusCode(result);
            EnsureHttpSuccessStatusCode(response);

            return result;
        }

        public async Task<CreateUserResponse> CreateUserAsync(
            CreateUserRequest user,
            CancellationToken cancellationToken
        )
        {
            logger
                .LogInformation("Received Create User request");

            var response =
                await
                    authenticatedApiClient
                        .PostAsJsonAsync(
                            "/api/users/create",
                            user,
                            cancellationToken
                        );

            var result =
                await
                    response
                        .TryParseResponse<CreateUserResponse>(
                            logger,
                            cancellationToken
                        );

            EnsureApiSuccessStatusCode(result);
            EnsureHttpSuccessStatusCode(response);

            return result;
        }
    }
}