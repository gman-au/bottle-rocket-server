using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Rocket.Notion.Infrastructure.Definition.Common;
using Rocket.Notion.Infrastructure.Definition.PageContent;

namespace Rocket.Notion.Infrastructure
{
    public class NotionNoteUploader(ILogger<NotionNoteUploader> logger) : BaseNotionClient, INotionNoteUploader
    {
        public async Task UploadTextNoteAsync(
            string integrationSecret,
            string parentNoteId,
            string textContent, 
            CancellationToken cancellationToken
            )
        {
            using var httpClient = GetBaseHttpClient(integrationSecret);

            var noteTitle = "Bottle Rocket note";
            
            var request = new PageContentRequest
            {
                Parent = new PageContentParent
                {
                    PageId = parentNoteId
                },
                Properties = new NotionProperties
                {
                    Title = new NotionPropertySet
                    {
                        Title =
                        [
                            new NotionProperty
                            {
                                Type = "text",
                                Text = new NotionTextProperty
                                {
                                    Content = noteTitle
                                }
                            }
                        ]
                    }
                },
                Children =
                [
                    new NotionParagraphBlock
                    {
                        RichText =
                        [
                            new NotionProperty
                            {
                                Type = "text",
                                Text = new NotionTextProperty
                                {
                                    Content = textContent
                                }
                            }
                        ]
                    }
                ]
            };
            
            logger
                .LogDebug("Sending POST Json: {json}", JsonSerializer.Serialize(request));

            var response =
                await
                    httpClient
                        .PostAsJsonAsync(
                            "v1/pages",
                            request,
                            cancellationToken
                        );

            response
                .EnsureSuccessStatusCode();
        }
    }
}