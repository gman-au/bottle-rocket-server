using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Rocket.Api.Contracts;
using Rocket.Api.Contracts.GlobalSettings;
using Rocket.Web.Client.Extensions;

namespace Rocket.Web.Client
{
    public partial class ApiRequestManager
    {
        public async Task<GlobalSettingsSpecifics> GetGlobalSettingsAsync(CancellationToken cancellationToken)
        {
            logger
                .LogInformation("Received Get global settings request");

            var response =
                await
                    authenticatedApiClient
                        .GetAsync(
                            "/api/globalSettings/get",
                            cancellationToken
                        );

            var result =
                await
                    response
                        .TryParseResponse<GlobalSettingsSpecifics>(
                            _jsonSerializerOptions,
                            logger,
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
            logger
                .LogInformation("Received Update global settings request");

            var response =
                await
                    authenticatedApiClient
                        .PostAsJsonAsync(
                            "/api/globalSettings/update",
                            globalSettings,
                            cancellationToken
                        );

            var result =
                await
                    response
                        .TryParseResponse<ApiResponse>(
                            _jsonSerializerOptions,
                            logger,
                            cancellationToken
                        );

            EnsureApiSuccessStatusCode(result);
            EnsureHttpSuccessStatusCode(response);

            return result;
        }
    }
}