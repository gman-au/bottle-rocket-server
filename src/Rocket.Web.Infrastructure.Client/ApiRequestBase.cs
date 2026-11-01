using System.Net.Http;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Rocket.Api.Contracts;
using Rocket.Domain.Enum;
using Rocket.Domain.Exceptions;
using Rocket.Interfaces;

namespace Rocket.Web.Infrastructure.Client
{
    internal class ApiRequestBase(
        ILogger logger,
        IJsonResolverInstanceProvider jsonResolverInstanceProvider,
        IAuthenticatedApiClient authenticatedApiClient
    )
    {
        protected readonly IAuthenticatedApiClient AuthenticatedApiClient = authenticatedApiClient;

        protected readonly JsonSerializerOptions JsonSerializerOptions =
            jsonResolverInstanceProvider
                .GetSerializationOptions();
        
        protected readonly ILogger Logger = logger;

        protected static void EnsureApiSuccessStatusCode(
            ApiResponse response
        )
        {
            if (response == null) return;

            if (response.ErrorCode != (int)ApiStatusCodeEnum.Ok)
                throw new RocketException(
                    response.ErrorMessage,
                    response.ErrorCode
                );
        }

        protected static void EnsureHttpSuccessStatusCode(
            HttpResponseMessage response
        )
        {
            try
            {
                response
                    .EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                var statusCode = (int)response.StatusCode;

                if (ex.StatusCode != null)
                    throw new RocketException(
                        $"The server returned a bad response: {(int)ex.StatusCode} ({ex.StatusCode})",
                        ApiStatusCodeEnum.ServerError,
                        statusCode
                    );
            }
        }
    }
}