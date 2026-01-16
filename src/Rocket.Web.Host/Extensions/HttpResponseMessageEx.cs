using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Rocket.Infrastructure;

namespace Rocket.Web.Host.Extensions
{
    public static class HttpResponseMessageEx
    {
        public static async Task<T> TryParseResponse<T>(
            this HttpResponseMessage httpResponseMessage,
            ILogger logger = null,
            CancellationToken cancellationToken = default
        )
        {
            try
            {
                var result =
                    await
                        httpResponseMessage
                            .Content
                            .ReadFromJsonAsync<T>(
                                RocketTypeInfoResolver
                                    .DefaultJsonSerializationOptions,
                                cancellationToken
                            );

                return result;
            }
            catch (JsonException)
            {
                var stringResponse =
                    await
                        httpResponseMessage
                            .Content
                            .ReadAsStringAsync(cancellationToken);

                var message =
                    $"Unexpected data received from endpoint: {stringResponse}";

                logger?
                    .LogError(message);

                return default;
            }
        }
    }
}