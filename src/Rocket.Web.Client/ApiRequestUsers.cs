using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Rocket.Api.Contracts.Users;
using Rocket.Interfaces;
using Rocket.Web.Client.Extensions;

namespace Rocket.Web.Client
{
    internal class ApiRequestUsers(
        ILogger<ApiRequestUsers> logger,
        IJsonResolverInstanceProvider jsonResolverInstanceProvider,
        IAuthenticatedApiClient authenticatedApiClient
    ) : ApiRequestBase(
        logger,
        jsonResolverInstanceProvider,
        authenticatedApiClient
    ), IApiRequestUsers
    {
        public async Task<FetchUsersResponse> GetUsersAsync(
            FetchUsersRequest request,
            CancellationToken cancellationToken
        )
        {
            Logger
                .LogInformation("Received get users request");

            var response =
                await
                    AuthenticatedApiClient
                        .PostAsJsonAsync(
                            "/api/users/fetch",
                            request,
                            cancellationToken
                        );

            var result =
                await
                    response
                        .TryParseResponse<FetchUsersResponse>(
                            JsonSerializerOptions,
                            Logger,
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
            Logger
                .LogInformation("Received Get User request");

            var response =
                await
                    AuthenticatedApiClient
                        .GetAsync(
                            $"/api/users/get/{id}",
                            cancellationToken
                        );

            var result =
                await
                    response
                        .TryParseResponse<UserSpecifics>(
                            JsonSerializerOptions,
                            Logger,
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
            Logger
                .LogInformation("Received Update User request");

            var response =
                await
                    AuthenticatedApiClient
                        .PostAsJsonAsync(
                            "/api/users/update",
                            user,
                            cancellationToken
                        );

            var result =
                await
                    response
                        .TryParseResponse<UpdateUserResponse>(
                            JsonSerializerOptions,
                            Logger,
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
            Logger
                .LogInformation("Received Create User request");

            var response =
                await
                    AuthenticatedApiClient
                        .PostAsJsonAsync(
                            "/api/users/create",
                            user,
                            cancellationToken
                        );

            var result =
                await
                    response
                        .TryParseResponse<CreateUserResponse>(
                            JsonSerializerOptions,
                            Logger,
                            cancellationToken
                        );

            EnsureApiSuccessStatusCode(result);
            EnsureHttpSuccessStatusCode(response);

            return result;
        }

        public async Task<UpdateUserResponse> UpdateDarkModeAsync(
            SetUserDarkModeRequest request,
            CancellationToken cancellationToken
        )
        {
            Logger
                .LogInformation("Received Update Dark Mode request");

            var response =
                await
                    AuthenticatedApiClient
                        .PostAsJsonAsync(
                            "/api/users/darkMode",
                            request,
                            cancellationToken
                        );

            var result =
                await
                    response
                        .TryParseResponse<UpdateUserResponse>(
                            JsonSerializerOptions,
                            Logger,
                            cancellationToken
                        );

            EnsureApiSuccessStatusCode(result);
            EnsureHttpSuccessStatusCode(response);

            return result;
        }
    }
}