using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Rocket.Domain.Enum;
using Rocket.Domain.Exceptions;
using Rocket.Domain.Executions;
using Rocket.Domain.Jobs;
using Rocket.Interfaces;
using Rocket.Replicate.Domain;
using Rocket.Replicate.Domain.Models.DataLabTo;

namespace Rocket.Replicate.Infrastructure.Models
{
    public class DataLabToExtractTextHook(
        ILogger<DataLabToExtractTextHook> logger,
        IReplicateClient replicateClient
    ) : IIntegrationHook
    {
        public bool IsApplicable(BaseExecutionStep step) => step is DataLabToExtractTextExecutionStep;

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

            if (step is not DataLabToExtractTextExecutionStep)
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

                // create the prediction
                var predictionId =
                    await
                        replicateClient
                            .CreatePredictionAsync(
                                apiToken,
                                null,
                                new DataLabToInput
                                {
                                    File = imageUrl
                                },
                                cancellationToken,
                                "v1/models/datalab-to/marker/predictions"
                            );

                // loop (this is a background job so patience is OK)


                var resultArtifact =
                    new ExecutionStepArtifact
                    {
                        Result = (int)ExecutionStatusEnum.Completed,
                        ArtifactDataFormat = (int)WorkflowFormatTypeEnum.RawTextData,
                        Artifact =
                            Encoding
                                .Default
                                .GetBytes(
                                    "dummy"
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
            }
        }
    }
}