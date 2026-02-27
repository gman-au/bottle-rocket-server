using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Rocket.Notion.Contracts;
using Rocket.Notion.Infrastructure.Definition.Searching;

namespace Rocket.Notion.Infrastructure
{
    public class NotionDataSourceSearcher : BaseNotionClient, INotionDataSourceSearcher
    {
        public async Task<IEnumerable<NotionDataSourceSummary>> GetDataSourcesAsync(
            string integrationSecret,
            CancellationToken cancellationToken
        )
        {
            using var httpClient = GetBaseHttpClient(integrationSecret);

            var request = new SearchRequest
            {
                Filter = new SearchFilter
                {
                    Property = "object",
                    Value = "data_source"
                }
            };

            var response =
                await
                    httpClient
                        .PostAsJsonAsync(
                            "v1/search",
                            request,
                            cancellationToken
                        );

            response
                .EnsureSuccessStatusCode();

            var notionResponse =
                await
                    response
                        .Content
                        .ReadFromJsonAsync<DataSourceSearchResponse>(cancellationToken);

            var searchResults = notionResponse?.Results ?? [];

            var results =
                searchResults
                    .Select(
                        o => new NotionDataSourceSummary
                        {
                            DataSourceId = o.Id,
                            DataSourceName =
                                (o.Title ?? [])
                                .FirstOrDefault()?
                                .Text?
                                .Content,
                            Fields =
                                o
                                    .Properties
                                    .Select(
                                        kvp =>
                                            new NotionDataSourceProperty
                                            {
                                                DataSourceId = kvp.Value?.Id,
                                                Name = kvp.Value?.Name
                                            }
                                    )
                        }
                    );

            return results;
        }
    }
}