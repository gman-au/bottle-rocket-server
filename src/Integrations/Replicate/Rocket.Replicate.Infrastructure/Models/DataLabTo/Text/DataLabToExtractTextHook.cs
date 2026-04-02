using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Rocket.Domain.Executions;
using Rocket.Domain.Jobs;
using Rocket.Domain.Utils;
using Rocket.Integrations.Common;
using Rocket.Integrations.Common.Extensions;
using Rocket.Interfaces;
using Rocket.Replicate.Domain;
using Rocket.Replicate.Domain.Models.DataLabTo;

namespace Rocket.Replicate.Infrastructure.Models.DataLabTo.Text
{
    public class DataLabToExtractTextHook(
        IReplicateClient replicateClient,
        IMarkdownStripper markdownStripper,
        IGlobalSettingsRepository globalSettingsRepository,
        ILogger<DataLabToExtractTextHook> logger
    ) : HookWithConnectorBase<DataLabToExtractTextExecutionStep, ReplicateConnector>(logger), IIntegrationHook
    {
        private const string DataLabToCustomEndpoint = "v1/models/datalab-to/marker/predictions";

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
                                    IncludeMetadata = false
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

                var extractedText =
                    result?.Markdown ?? string.Empty;

                logger
                    .LogDebug(
                        "Extracted markdown: {extractedText}",
                        extractedText
                    );

                var strippedText =
                    markdownStripper
                        .StripFooter(extractedText);

                logger
                    .LogDebug(
                        "Stripped markdown: {strippedText}",
                        strippedText
                    );
                
                await
                    appendLogMessageCallback(
                        step.Id,
                        strippedText
                    );

                return
                    strippedText
                        .AsCompletedRawTextArtifact();
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