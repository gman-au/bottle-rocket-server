using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Rocket.Notion.Infrastructure.Definition.Common;
using Rocket.Notion.Infrastructure.Definition.FileUpload;
using Rocket.Notion.Infrastructure.Definition.PageContent;

namespace Rocket.Notion.Infrastructure
{
    public class NotionNoteUploader(ILogger<NotionNoteUploader> logger) : BaseNotionClient, INotionNoteUploader
    {
        public async Task UploadTextNoteAsync(
            string integrationSecret,
            string parentNoteId,
            string title,
            string textContent,
            CancellationToken cancellationToken
        )
        {
            using var httpClient = GetBaseHttpClient(integrationSecret);

            var noteTitle = title ?? $"Bottle Rocket note {DateTime.UtcNow.ToLocalTime():f}";

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
                        Paragraph = new NotionParagraph
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
                    }
                ]
            };

            logger
                .LogDebug(
                    "Sending POST Json: {json}",
                    JsonSerializer.Serialize(request)
                );

            HttpResponseMessage response = null;
            try
            {
                response =
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
            catch (HttpRequestException)
            {
                await
                    HandleNotionExceptionAsync(
                        response,
                        cancellationToken
                    );
            }
        }

        public async Task UploadImageNoteAsync(
            string integrationSecret,
            string parentNoteId,
            string title,
            string fileUploadId,
            CancellationToken cancellationToken
        )
        {
            using var httpClient = GetBaseHttpClient(integrationSecret);

            var noteTitle = title ?? $"Bottle Rocket note {DateTime.Now:f}";

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
                    new NotionImageFileUploadBlock
                    {
                        Image = new NotionUploadedImageFile
                        {
                            FileUpload = new FileUpload
                            {
                                Id = fileUploadId
                            }
                        }
                    }
                ]
            };

            logger
                .LogDebug(
                    "Sending POST Json: {json}",
                    JsonSerializer.Serialize(request)
                );

            HttpResponseMessage response = null;
            try
            {
                response =
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
            catch (HttpRequestException)
            {
                await
                    HandleNotionExceptionAsync(
                        response,
                        cancellationToken
                    );
            }
        }
    }
}