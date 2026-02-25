using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Rocket.Domain.Enum;
using Rocket.Domain.Exceptions;
using Rocket.Domain.Executions;
using Rocket.Domain.Jobs;
using Rocket.Interfaces;
using Rocket.Page.Schemas.ProjectTaskTracker;
using Rocket.Replicate.Domain;
using Rocket.Replicate.Domain.Models.DataLabTo;

namespace Rocket.Replicate.Infrastructure.Models.DataLabTo.Project
{
    public class DataLabToExtractProjectHook(
        ILogger<DataLabToExtractProjectHook> logger,
        ISchemaResponseBuilder schemaResponseBuilder,
        IReplicateClient replicateClient
    ) : IIntegrationHook
    {
        private const string DataLabToCustomEndpoint = "v1/models/datalab-to/marker/predictions";

        private const string HardCodedSchemaString =
            """
            {
              "type": "object",
              "properties": {
                "notes": { "type": "string", "description": "Any notes text below the table" },
                "items": {
                  "type": "array",
                  "description": "Each row in the project task table",
                  "items": {
                    "type": "object",
                    "properties": {
                      "project": { "type": "string", "description": "Project code from the PROJECT column" },
                      "task":    { "type": "string", "description": "Task description from the TASK column" },
                      "due_date":{ "type": "string", "description": "Due date from the DUE column" },
                      "est_time":{ "type": "string", "description": "Estimated time from the EST. TIME column" }
                    }
                  }
                }
              },
              "required": ["items"]
            }
            """; 
        
        public bool IsApplicable(BaseExecutionStep step) => step is DataLabToExtractProjectExecutionStep;

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

            if (step is not DataLabToExtractProjectExecutionStep)
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
                                null,
                                new DataLabToInput
                                {
                                    File = imageUrl,
                                    OutputFormat = "json",
                                    DisableImageExtraction = true,
                                    IncludeMetadata = true,
                                    PageSchema = HardCodedSchemaString,
                                    UseLlm = true
                                },
                                cancellationToken,
                                DataLabToCustomEndpoint
                            );
                
                logger
                    .LogInformation("Created prediction in Replicate: {predictionId}", predictionId);

                var result =
                    await
                        replicateClient
                            .WaitUntilPredictionCompletesAsync<DataLabToOutput>(
                                apiToken,
                                predictionId,
                                cancellationToken
                            );

                var extractedJson = result?.ExtractionSchemaJson ?? string.Empty;

                var data =
                    schemaResponseBuilder
                        .Build(
                            extractedJson,
                            RocketbookPageTemplateTypeEnum.ProjectTaskTracker
                        );
                
                if (data is not ProjectTaskTrackerSchema projectTaskTrackerSchema)
                    throw new RocketException(
                        $"There was an error parsing the data returned from the model to type {typeof(ProjectTaskTrackerSchema)}.",
                        ApiStatusCodeEnum.ThirdPartyServiceError
                    );
                
                var responseJson =
                    JsonSerializer
                        .Serialize(projectTaskTrackerSchema);
                
                await
                    appendLogMessageCallback(
                        step.Id,
                        responseJson
                    );
                
                var resultArtifact =
                    new ExecutionStepArtifact
                    {
                        Result = (int)ExecutionStatusEnum.Completed,
                        ArtifactDataFormat = (int)WorkflowFormatTypeEnum.ProjectTaskTrackerData,
                        Artifact =
                            Encoding
                                .Default
                                .GetBytes(
                                    responseJson
                                ),
                        FileExtension = ".json"
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