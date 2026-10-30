using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Rocket.Api.Contracts;
using Rocket.Api.Contracts.GlobalSettings;
using Rocket.Interfaces;
using Rocket.Web.Infrastructure.Client.Extensions;

namespace Rocket.Web.Infrastructure.Client
{
    internal class ApiRequestGlobalSettings(
        ILogger<ApiRequestGlobalSettings> logger,
        IJsonResolverInstanceProvider jsonResolverInstanceProvider,
        IAuthenticatedApiClient authenticatedApiClient
    ) : ApiRequestBase(
        logger,
        jsonResolverInstanceProvider,
        authenticatedApiClient
    ), IApiRequestGlobalSettings
    {
        public async Task<GlobalSettingsSpecifics> GetGlobalSettingsAsync(
            CancellationToken cancellationToken
        )
        {
            Logger
                .LogInformation("Received Get global settings request");

            var response =
                await
                    AuthenticatedApiClient
                        .GetAsync(
                            "/api/globalSettings/get",
                            cancellationToken
                        );

            var result =
                await
                    response
                        .TryParseResponse<GlobalSettingsSpecifics>(
                            JsonSerializerOptions,
                            Logger,
                            cancellationToken
                        );

            EnsureApiSuccessStatusCode(result);
            EnsureHttpSuccessStatusCode(response);

            return result;
        }

        public async Task<ApiResponse> UpdateGlobalSettingsAsync(
            GlobalSettingsSpecifics globalSettings,
            CancellationToken cancellationToken
        )
        {
            Logger
                .LogInformation("Received Update global settings request");

            var response =
                await
                    AuthenticatedApiClient
                        .PostAsJsonAsync(
                            "/api/globalSettings/update",
                            globalSettings,
                            cancellationToken
                        );

            var result =
                await
                    response
                        .TryParseResponse<ApiResponse>(
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