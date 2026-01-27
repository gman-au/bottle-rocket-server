using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Rocket.Domain.Enum;
using Rocket.Domain.Exceptions;
using Rocket.Notion.Infrastructure.Definition.Common;

namespace Rocket.Notion.Infrastructure
{
    public class BaseNotionClient
    {
        private const string NotionEndpoint = "https://api.notion.com";
        private const string NotionVersion = "2022-06-28";

        protected HttpClient GetBaseHttpClient(string integrationSecret)
        {
            var httpClient = new HttpClient();

            httpClient.BaseAddress =
                new Uri(NotionEndpoint);

            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(
                    "Bearer",
                    integrationSecret
                );

            httpClient.DefaultRequestHeaders.Add(
                "Notion-Version",
                NotionVersion
            );

            return httpClient;
        }

        protected static readonly JsonSerializerOptions DefaultJsonSerializationOptions = new()
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        protected async Task HandleNotionExceptionAsync(
            HttpResponseMessage responseMessage,
            CancellationToken cancellationToken
        )
        {
            var parsedError =
                await
                    responseMessage?
                        .Content?
                        .ReadFromJsonAsync<NotionErrorResponse>(cancellationToken);

            throw new RocketException(
                parsedError?.Message ?? "There was an error uploading the image",
                ApiStatusCodeEnum.ThirdPartyServiceError
            );
        }
    }
}