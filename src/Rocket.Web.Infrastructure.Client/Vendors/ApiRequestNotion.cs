using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Rocket.Interfaces;
using Rocket.Notion.Contracts;
using Rocket.Web.Infrastructure.Client.Extensions;

namespace Rocket.Web.Infrastructure.Client.Vendors
{
    internal class ApiRequestNotion(
        ILogger<ApiRequestNotion> logger,
        IJsonResolverInstanceProvider jsonResolverInstanceProvider,
        IAuthenticatedApiClient authenticatedApiClient
    ) : ApiRequestBase(
        logger,
        jsonResolverInstanceProvider,
        authenticatedApiClient
    ), IApiRequestNotion
    {
        public async Task<GetAllNotionParentNotesResponse> GetNotionParentNotesAsync(
            GetAllNotionParentNotesRequest request,
            CancellationToken cancellationToken
        )
        {
            Logger
                .LogInformation("Received Get (Notion) parent notes request");

            var response =
                await
                    AuthenticatedApiClient
                        .PostAsJsonAsync(
                            "/api/notion/workflows/getParentNotes",
                            request,
                            cancellationToken
                        );

            var result =
                await
                    response
                        .TryParseResponse<GetAllNotionParentNotesResponse>(
                            JsonSerializerOptions,
                            Logger,
                            cancellationToken
                        );

            EnsureApiSuccessStatusCode(result);
            EnsureHttpSuccessStatusCode(response);

            return result;
        }

        public async Task<GetAllNotionDataSourcesResponse> GetNotionDataSourcesAsync(
            GetAllNotionDataSourcesRequest request,
            CancellationToken cancellationToken
        )
        {
            Logger
                .LogInformation("Received Get (Notion) data sources request");

            var response =
                await
                    AuthenticatedApiClient
                        .PostAsJsonAsync(
                            "/api/notion/workflows/getDataSources",
                            request,
                            cancellationToken
                        );

            var result =
                await
                    response
                        .TryParseResponse<GetAllNotionDataSourcesResponse>(
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