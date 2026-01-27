using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Rocket.Notion.Contracts;
using Rocket.Notion.Infrastructure.Definition.Searching;

namespace Rocket.Notion.Infrastructure
{
    public class NotionNoteSearcher : BaseNotionClient, INotionNoteSearcher
    {
        public async Task<IEnumerable<NotionParentNoteSummary>> GetParentNotesAsync(
            string integrationSecret,
            CancellationToken cancellationToken
        )
        {
            using var httpClient = GetBaseHttpClient(integrationSecret);
            
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

        public Task UploadTextNoteAsync(
            string textContent, 
            CancellationToken cancellationToken
            )
        {
            throw new NotImplementedException();
        }
    }
}