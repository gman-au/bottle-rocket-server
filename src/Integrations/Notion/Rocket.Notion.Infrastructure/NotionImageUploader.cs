using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Rocket.Domain.Enum;
using Rocket.Domain.Exceptions;
using Rocket.Domain.Jobs;
using Rocket.Integrations.Common.Extensions;
using Rocket.Interfaces;
using Rocket.Notion.Infrastructure.Definition.FileUpload;

namespace Rocket.Notion.Infrastructure
{
    public class NotionImageUploader(
        ILogger<NotionNoteUploader> logger,
        INotionNoteUploader notionNoteUploader
    ) : BaseNotionClient, INotionImageUploader
    {
        public async Task UploadImageNoteAsync(
            string integrationSecret,
            string parentNoteId,
            IEnumerable<ArtifactDataElement> elements,
            IJobControl jobControl,
            CancellationToken cancellationToken
        )
        {
            using var httpClient = GetBaseHttpClient(integrationSecret);

            var allCompleted = true;

            foreach (var element in elements)
            {
                try
                {
                    await
                        jobControl
                            .StartStepAsync(
                                element.ScanId
                            );

                    var noteTitle = element.FileName ?? $"BR_Note_{DateTime.Now:yyyy_MM_dd_HH_mm_ss}";

                    var fileName = noteTitle + element.FileExtension;

                    var createUploadRequest =
                        new CreateUploadFileRequest
                        {
                            Mode = "single_part",
                            FileName = fileName,
                        };

                    logger
                        .LogDebug(
                            "Sending POST Json: {json}",
                            JsonSerializer.Serialize(createUploadRequest)
                        );

                    HttpResponseMessage createUploadResponse = null;
                    try
                    {
                        createUploadResponse =
                            await
                                httpClient
                                    .PostAsJsonAsync(
                                        "v1/file_uploads",
                                        createUploadRequest,
                                        DefaultJsonSerializationOptions,
                                        cancellationToken
                                    );

                        createUploadResponse
                            .EnsureSuccessStatusCode();
                    }
                    catch (HttpRequestException)
                    {
                        await
                            HandleNotionExceptionAsync(
                                createUploadResponse,
                                cancellationToken
                            );
                    }

                    var parsedCreateUploadResponse =
                        await
                            createUploadResponse?
                                .Content?
                                .ReadFromJsonAsync<CreateUploadFileResponse>(cancellationToken);

                    var uploadId = parsedCreateUploadResponse?.Id;

                    if (uploadId == null)
                        throw new RocketException(
                            "There was an error uploading the image to Notion.",
                            ApiStatusCodeEnum.ThirdPartyServiceError
                        );

                    using var form = new MultipartFormDataContent();

                    var fileContent = new ByteArrayContent(element.Artifact);

                    fileContent.Headers.ContentType =
                        new MediaTypeHeaderValue(parsedCreateUploadResponse.ContentType);

                    form
                        .Add(
                            fileContent,
                            "file",
                            fileName
                        );

                    HttpResponseMessage sendUploadResponse = null;
                    try
                    {
                        sendUploadResponse =
                            await
                                httpClient
                                    .PostAsync(
                                        $"/v1/file_uploads/{uploadId}/send",
                                        form,
                                        cancellationToken
                                    );

                        sendUploadResponse
                            .EnsureSuccessStatusCode();
                    }
                    catch (HttpRequestException)
                    {
                        await
                            HandleNotionExceptionAsync(
                                sendUploadResponse,
                                cancellationToken
                            );
                    }

                    await
                        notionNoteUploader
                            .UploadImageNoteAsync(
                                integrationSecret,
                                parentNoteId,
                                noteTitle,
                                uploadId,
                                cancellationToken
                            );

                    await
                        jobControl
                            .SucceedStepAsync(
                                $"File {fileName} uploaded to Notion",
                                element.ScanId
                            );
                }
                catch (Exception ex)
                {
                    allCompleted = false;

                    await
                        jobControl
                            .FailStepAsync(
                                ex,
                                element.ScanId
                            );
                }
            }
            
            if (!allCompleted)
            {
                throw new RocketException(
                    "There was an error uploading the data to Notion",
                    ApiStatusCodeEnum.ThirdPartyServiceError
                );
            }
        }
    }
}