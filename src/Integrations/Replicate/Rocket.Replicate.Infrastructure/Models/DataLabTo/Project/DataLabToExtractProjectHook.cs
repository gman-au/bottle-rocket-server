using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Rocket.Domain.Enum;
using Rocket.Domain.Exceptions;
using Rocket.Domain.Executions;
using Rocket.Domain.Jobs;
using Rocket.Integrations.Common;
using Rocket.Integrations.Common.Extensions;
using Rocket.Interfaces;
using Rocket.Page.Schemas.ProjectTaskTracker;
using Rocket.Replicate.Domain;
using Rocket.Replicate.Domain.Models.DataLabTo;

namespace Rocket.Replicate.Infrastructure.Models.DataLabTo.Project
{
    public class DataLabToExtractProjectHook(
        ISchemaResponseBuilder schemaResponseBuilder,
        IReplicateClient replicateClient,
        ILogger<DataLabToExtractProjectHook> logger
    ) : HookWithConnectorBase<DataLabToExtractProjectExecutionStep, ReplicateConnector>(logger), IIntegrationHook
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
                                cancellationToken
                            );

                var extractedJson =
                    result?
                        .ExtractionSchemaJson ?? string.Empty;

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

                await
                    appendLogMessageCallback(
                        step.Id,
                        JsonSerializer
                            .Serialize(projectTaskTrackerSchema)
                    );

                var resultArtifact =
                    projectTaskTrackerSchema
                        .AsCompletedProjectTaskTrackerDataArtifact();

                return resultArtifact;
            }
            finally
            {
                // Delete uploads
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