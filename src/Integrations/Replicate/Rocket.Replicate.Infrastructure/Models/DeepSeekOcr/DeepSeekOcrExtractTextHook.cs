using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Rocket.Domain.Enum;
using Rocket.Domain.Exceptions;
using Rocket.Domain.Executions;
using Rocket.Domain.Jobs;
using Rocket.Domain.Utils;
using Rocket.Interfaces;
using Rocket.Replicate.Domain;
using Rocket.Replicate.Domain.Models.DeepSeekOcr;

namespace Rocket.Replicate.Infrastructure.Models.DeepSeekOcr
{
    public class DeepSeekOcrExtractTextHook(
        ILogger<DeepSeekOcrExtractTextHook> logger,
        IGlobalSettingsRepository globalSettingsRepository,
        IReplicateClient replicateClient
    ) : IIntegrationHook
    {
        private const string DeepSeekModelVersion = "a524caeaa23495bc9edc805ab08ab5fe943afd3febed884a4f3747aa32e9cd61";
        
        public bool IsApplicable(BaseExecutionStep step) => step is DeepSeekOcrExtractTextExecutionStep;

        public async Task<ExecutionStepArtifact> ProcessAsync(
            IWorkflowExecutionContext context,
            BaseExecutionStep step,
            string userId,
            Func<string, string, Task> appendLogMessageCallback,
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

            var imageBytes = artifact.Artifact;

            if (step is not DeepSeekOcrExtractTextExecutionStep)
                throw new RocketException(
                    "Unexpected step format, please check configuration",
                    ApiStatusCodeEnum.DeveloperError
                );

            var apiToken =
                connector
                    .ApiToken;

            var fileName = $"{Guid.NewGuid()}{artifact.FileExtension}";

            var imageIdToDelete = string.Empty;

            try
            {
                // upload the file
                var (imageUrl, imageId) =
                    await
                        replicateClient
                            .UploadFileAsync(
                                apiToken,
                                imageBytes,
                                fileName,
                                artifact.FileExtension,
                                cancellationToken
                            );

                imageIdToDelete = imageId;
                
                logger
                    .LogInformation("Uploaded image to Replicate: {imageUrl}", imageUrl);

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
                    .LogInformation("Created prediction in Replicate: {predictionId}", predictionId);

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
                            .WaitUntilPredictionCompletesAsync(
                                apiToken,
                                predictionId,
                                timeoutInMinutes,
                                cancellationToken
                            );

                var extractedText = result ?? string.Empty;

                await
                    appendLogMessageCallback(
                        step.Id,
                        extractedText
                    );
                
                var resultArtifact =
                    new ExecutionStepArtifact
                    {
                        Result = (int)ExecutionStatusEnum.Completed,
                        ArtifactDataFormat = (int)WorkflowFormatTypeEnum.RawTextData,
                        Artifact =
                            Encoding
                                .Default
                                .GetBytes(
                                    extractedText
                                ),
                        FileExtension = ".txt"
                    };

                return resultArtifact;
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
                    .LogInformation("Deleted image in Replicate: {imageIdToDelete}", imageIdToDelete);
            }
        }
    }
}