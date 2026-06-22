using System;
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
    ) : HookWithConnectorBase<DataLabToExtractTextExecutionStep, ReplicateConnector>(logger, fileRetitler), IIntegrationHook
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
            Func<string, string, Task> appendLogMessageCallback,
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

            var imageBytes =
                Artifact
                    .Artifact;

            var apiToken =
                Connector
                    .ApiToken;

            var fileName = $"{Guid.NewGuid()}{Artifact.FileExtension}";

            var imageIdToDelete = string.Empty;

            try
            {
                // Upload the file
                var (imageUrl, imageId) =
                    await
                        replicateClient
                            .UploadFileAsync(
                                apiToken,
                                imageBytes,
                                fileName,
                                Artifact.FileExtension,
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

                var globalSettings =
                    await
                        globalSettingsRepository
                            .GetGlobalSettingsAsync(cancellationToken);

                var timeoutInMinutes =
                    globalSettings?.DefaultModelTimeoutInMinutes ??
                    DomainConstants.GlobalDefaultModelTimeoutInMinutes;
                
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
                
                RetitleFileIfApplicable(extractedMarkdown);

                logger
                    .LogDebug(
                        "Extracted markdown: {extractedMarkdown}",
                        extractedMarkdown
                    );
                
                await
                    appendLogMessageCallback(
                        step.Id,
                        extractedMarkdown
                    );

                return
                    extractedMarkdown
                        .AsCompletedRawTextArtifact(Artifact);
            }
            finally
            {
                // Delete upload
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
    }
}