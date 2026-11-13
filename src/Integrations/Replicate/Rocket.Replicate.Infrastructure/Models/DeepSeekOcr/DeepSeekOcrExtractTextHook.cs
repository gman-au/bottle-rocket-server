using System;
using System.Collections.Generic;
using System.Text;
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
using Rocket.Replicate.Domain;
using Rocket.Replicate.Domain.Models.DeepSeekOcr;

namespace Rocket.Replicate.Infrastructure.Models.DeepSeekOcr
{
    public class DeepSeekOcrExtractTextHook(
        IFileRetitler fileRetitler,
        ILogger<DeepSeekOcrExtractTextHook> logger,
        IGlobalSettingsRepository globalSettingsRepository,
        IReplicateClient replicateClient
    ) : IIntegrationHook
    {
        private const string DeepSeekModelVersion = "a524caeaa23495bc9edc805ab08ab5fe943afd3febed884a4f3747aa32e9cd61";

        public bool IsApplicable(
            BaseExecutionStep step
        ) => step is DeepSeekOcrExtractTextExecutionStep;

        public async Task<ExecutionStepArtifact> ProcessAsync(
            IWorkflowExecutionContext context,
            BaseExecutionStep step,
            string userId,
            IJobControl jobControl,
            CancellationToken cancellationToken
        )
        {
            var artifact =
                context
                    .GetInputArtifact();

            var connector =
                await
                    context
                        .GetConnectorAsync<ReplicateConnector>(
                            userId,
                            step,
                            cancellationToken
                        );

            var artifactElements = new List<ArtifactDataElement>();

            var elements =
                artifact
                    .Elements;

            if (step is not DeepSeekOcrExtractTextExecutionStep)
                throw new RocketException(
                    "Unexpected step format, please check configuration",
                    ApiStatusCodeEnum.DeveloperError
                );

            var apiToken =
                connector
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

                    var fileName = $"{element.FileName}{element.FileExtension}";

                    // upload the file
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

                    // create the prediction
                    var predictionId =
                        await
                            replicateClient
                                .CreatePredictionAsync(
                                    apiToken,
                                    DeepSeekModelVersion,
                                    new DeepSeekOcrInput
                                    {
                                        Image = imageUrl
                                    },
                                    cancellationToken
                                );

                    logger
                        .LogInformation(
                            "Created prediction in Replicate: {predictionId}",
                            predictionId
                        );

                    var result =
                        await
                            replicateClient
                                .WaitUntilPredictionCompletesAsync(
                                    apiToken,
                                    predictionId,
                                    timeoutInMinutes,
                                    cancellationToken
                                );

                    var extractedText = result ?? string.Empty;

                    await
                        jobControl
                            .SucceedStepAsync(
                                $"DeepSeek OCR response: \"{extractedText}\"",
                                element.ScanId
                            );

                    var resultArtifact =
                        new ArtifactDataElement
                        {
                            Artifact =
                                Encoding
                                    .Default
                                    .GetBytes(
                                        extractedText
                                    ),
                            FileExtension = CommonFileConstants.TextFileExtension,
                            ScanId = element.ScanId,
                            FileName =
                                await
                                    fileRetitler
                                        .RetitleAsync(
                                            extractedText,
                                            element.ScanId,
                                            artifact.UserId,
                                            cancellationToken
                                        ) ?? artifact.FileName
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
                    BatchId = artifact.BatchId,
                    Elements = uniqueElements,
                    FileName = artifact.FileName,
                    UserId = artifact.UserId
                };
        }
    }
}