using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Rocket.Api.Contracts;
using Rocket.Web.Host.Extensions;

namespace Rocket.Web.Host.Api
{
    public partial class ApiRequestManager
    {
        public async Task<FetchConnectorsResponse> GetMyConnectorsAsync(
            FetchConnectorsRequest request, 
            CancellationToken cancellationToken
        )
        {
            logger
                .LogInformation("Received get connectors request");

            var response =
                await
                    authenticatedApiClient
                        .PostAsJsonAsync(
                            $"/api/connectors/fetch",
                            request,
                            cancellationToken
                        );

            var result =
                await
                    response
                        .TryParseResponse<FetchConnectorsResponse>(
                            logger,
                            cancellationToken
                        );

            EnsureApiSuccessStatusCode(result);
            EnsureHttpSuccessStatusCode(response);

            return result;
        }

        /*public async Task<UpdateUserResponse> UpdateUserAsync(
            UserDetail user,
            CancellationToken cancellationToken)
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
        }*/
    }
}