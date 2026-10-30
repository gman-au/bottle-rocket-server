using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Rocket.Api.Contracts;
using Rocket.Interfaces;
using Rocket.Web.Infrastructure.Client.Extensions;

namespace Rocket.Web.Infrastructure.Client
{
    internal class ApiRequestSystem(
        ILogger<ApiRequestSystem> logger,
        IJsonResolverInstanceProvider jsonResolverInstanceProvider,
        IAuthenticatedApiClient authenticatedApiClient
    ) : ApiRequestBase(
        logger,
        jsonResolverInstanceProvider,
        authenticatedApiClient
    ), IApiRequestSystem
    {
        public async Task<StartupPhaseResponse> GetStartupPhaseAsync(CancellationToken cancellationToken)
        {
            Logger
                .LogInformation("Checking startup phase");

            var response =
                await
                    AuthenticatedApiClient
                        .GetAsync(
                            "/api/startup/phase",
                            cancellationToken
                        );

            response
                .EnsureSuccessStatusCode();

            var result =
                await
                    response
                        .TryParseResponse<StartupPhaseResponse>(
                            JsonSerializerOptions,
                            Logger,
                            cancellationToken
                        );

            return result;
        }
        
        public async Task<VersionResponse> GetSystemVersionsAsync(
            CancellationToken cancellationToken
        )
        {
            Logger
                .LogInformation("Checking system versions");

            var webVersion =
                Environment.GetEnvironmentVariable("APP_VERSION")
                ?? "unknown";

            var response =
                await
                    AuthenticatedApiClient
                        .GetAsync(
                            "/api/version",
                            cancellationToken
                        );

            var result =
                await
                    response
                        .TryParseResponse<VersionResponse>(
                            JsonSerializerOptions,
                            Logger,
                            cancellationToken
                        );

            result.WebVersion = webVersion;

            return result;
        }
    }
}