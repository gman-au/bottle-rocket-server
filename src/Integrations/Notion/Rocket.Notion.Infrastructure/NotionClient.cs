using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Rocket.Notion.Contracts;
using Rocket.Notion.Infrastructure.Definition;

namespace Rocket.Notion.Infrastructure
{
    public class NotionClient : INotionClient
    {
        private const string NotionEndpoint = "https://api.notion.com";
        private const string NotionVersion = "2022-06-28";
        private const string NotionContentType = "application/json";

        public async Task<IEnumerable<NotionParentNoteSummary>> GetParentNotesAsync(
            string integrationSecret,
            CancellationToken cancellationToken
        )
        {
            using var httpClient = new HttpClient();

            httpClient.BaseAddress =
                new Uri(NotionEndpoint);

            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(
                    "Bearer",
                    integrationSecret
                );

            httpClient.DefaultRequestHeaders.Add("Notion-Version", NotionVersion);
            // httpClient.DefaultRequestHeaders.Add("Content-Type", NotionContentType);
            
            var request = new PageSearchRequest
            {
                Filter = new SearchFilter
                {
                    Property = "object",
                    Value = "page"
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
                        .ReadFromJsonAsync<PageSearchResponse>(cancellationToken);

            var searchResults = notionResponse?.Results ?? [];

            var results =
                searchResults
                    .Select(
                        o => new NotionParentNoteSummary
                        {
                            ParentNoteId = o.Id,
                            ParentNoteName =
                                (o.Properties?.Title?.Title ?? [])
                                .FirstOrDefault()?
                                .Text?
                                .Content
                        }
                    );

            return results;
        }
    }
}