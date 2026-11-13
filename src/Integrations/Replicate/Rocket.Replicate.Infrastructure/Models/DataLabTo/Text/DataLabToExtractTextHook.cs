using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Rocket.Domain.Enum;
using Rocket.Domain.Exceptions;
using Rocket.Domain.Executions;
using Rocket.Domain.Jobs;
using Rocket.Domain.Utils;
using Rocket.Integrations.Common;
using Rocket.Integrations.Common.Extensions;
using Rocket.Interfaces;
using Rocket.Page.Schemas.FallbackText;
using Rocket.Replicate.Domain;
using Rocket.Replicate.Domain.Models.DataLabTo;

namespace Rocket.Replicate.Infrastructure.Models.DataLabTo.Text
{
    public class DataLabToExtractTextHook(
        IReplicateClient replicateClient,
        IGlobalSettingsRepository globalSettingsRepository,
        IFileRetitler fileRetitler,
        ILogger<DataLabToExtractTextHook> logger
    ) : HookWithConnectorBase<DataLabToExtractTextExecutionStep, ReplicateConnector>(
        logger
    ), IIntegrationHook
    {
        private const string DataLabToCustomEndpoint = "v1/models/datalab-to/marker/predictions";

        private const string HardCodedSchemaString =
            """
            {
              "type": "object",
              "properties": {
                "main_page": { "type": "string", "description": "All text in the main page, preserved in markdown with newlines and spaces where applicable." },
                "bottom_navigation_bar": { "type": "string", "description": "All text and images in the bottom navigation bar." },
                "qr_code": { "type": "string", "description": "A QR code image, if it exists." }
              },
              "required": ["main_page"]
            }
            """;

        public async Task<ExecutionStepArtifact> ProcessAsync(
            IWorkflowExecutionContext context,
            BaseExecutionStep step,
            string userId,
            IJobControl jobControl,
            CancellationToken cancellationToken
        )
        {
            context
                .InitializeStep(
                    this,
                    step
                )
                .InitializeArtifact(this)
                .InitializeConnector(
                    this,
                    userId,
                    step,
                    cancellationToken
                );

            var artifactElements = new List<ArtifactDataElement>();

            var elements =
                Artifact
                    .Elements;

            var apiToken =
                Connector
                    .ApiToken;

            var allCompleted = true;

            var globalSettings =
                await
                    globalSettingsRepository
                        .GetGlobalSettingsAsync(cancellationToken);

            var timeoutInMinutes =
                globalSettings?.DefaultModelTimeoutInMinutes ??
                DomainConstants.GlobalDefaultModelTimeoutInMinutes;

            foreach (var element in elements)
            {
                var imageIdToDelete = string.Empty;

                try
                {
                    await
                        jobControl
                            .StartStepAsync(
                                element.ScanId
                            );

                    var fileName = $"{Guid.NewGuid()}{element.FileExtension}";

                    // Upload the file
                    var (imageUrl, imageId) =
                        await
                            replicateClient
                                .UploadFileAsync(
                                    apiToken,
                                    element.Artifact,
                                    fileName,
                                    element.FileExtension,
                                    cancellationToken
                                );

                    imageIdToDelete = imageId;

                    logger
                        .LogInformation(
                            "Uploaded image to Replicate: {imageUrl}",
                            imageUrl
                        );

                    // Create the prediction
                    var predictionId =
                        await
                            replicateClient
                                .CreatePredictionAsync(
                                    apiToken,
                                    null,
                                    new DataLabToInput
                                    {
                                        File = imageUrl,
                                        DisableImageExtraction = true,
                                        UseLlm = true,
                                        SkipCache = true,
                                        ForceOcr = true,
                                        IncludeMetadata = false,
                                        PageSchema = HardCodedSchemaString
                                    },
                                    cancellationToken,
                                    DataLabToCustomEndpoint
                                );

                    logger
                        .LogInformation(
                            "Created prediction in Replicate: {predictionId}",
                            predictionId
                        );

                    var result =
                        await
                            replicateClient
                                .WaitUntilPredictionCompletesAsync<DataLabToOutput>(
                                    apiToken,
                                    predictionId,
                                    timeoutInMinutes,
                                    cancellationToken
                                );

                    var extractedJson = result?.ExtractionSchemaJson;

                    if (string.IsNullOrEmpty(extractedJson))
                        throw new RocketException(
                            "The extracted text was empty. Please try again.",
                            ApiStatusCodeEnum.ThirdPartyServiceError
                        );

                    var fallbackTextSchema =
                        JsonSerializer
                            .Deserialize<FallbackTextSchema>(extractedJson);

                    var extractedMarkdown =
                        fallbackTextSchema?.MainPage ?? string.Empty;

                    logger
                        .LogDebug(
                            "Extracted markdown: {extractedMarkdown}",
                            extractedMarkdown
                        );

                    await
                        jobControl
                            .SucceedStepAsync(
                                $"DataLabTo OCR response: \"{extractedMarkdown}\"",
                                element.ScanId
                            );

                    var resultArtifact =
                        new ArtifactDataElement
                        {
                            Artifact =
                                Encoding
                                    .Default
                                    .GetBytes(
                                        extractedMarkdown
                                    ),
                            FileExtension = CommonFileConstants.TextFileExtension,
                            ScanId = element.ScanId,
                            FileName =
                                await
                                    fileRetitler
                                        .RetitleAsync(
                                            extractedMarkdown,
                                            element.ScanId,
                                            Artifact.UserId,
                                            cancellationToken
                                        ) ?? Artifact.FileName
                        };

                    artifactElements
                        .Add(resultArtifact);
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
                finally
                {
                    // delete uploads
                    if (!string.IsNullOrEmpty(imageIdToDelete))
                        await
                            replicateClient
                                .DeleteUploadAsync(
                                    apiToken,
                                    imageIdToDelete
                                );

                    logger
                        .LogInformation(
                            "Deleted image in Replicate: {imageIdToDelete}",
                            imageIdToDelete
                        );
                }
            }

            if (!allCompleted)
            {
                throw new RocketException(
                    "There was an error extracting the text via Replicate",
                    ApiStatusCodeEnum.ThirdPartyServiceError
                );
            }

            // Retitle
            var uniqueElements =
                artifactElements
                    .ToArray()
                    .EnumerateUniquely();

            return
                new ExecutionStepArtifact
                {
                    Result = (int)ExecutionStatusEnum.Completed,
                    ArtifactDataFormat = (int)WorkflowFormatTypeEnum.RawTextData,
                    BatchId = Artifact.BatchId,
                    Elements = uniqueElements,
                    FileName = Artifact.FileName,
                    UserId = Artifact.UserId
                };
        }
    }
}