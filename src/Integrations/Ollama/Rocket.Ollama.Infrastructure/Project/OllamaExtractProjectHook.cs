using System;
using System.Configuration;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Rocket.Domain.Enum;
using Rocket.Domain.Exceptions;
using Rocket.Domain.Executions;
using Rocket.Domain.Jobs;
using Rocket.Interfaces;
using Rocket.Ollama.Domain;
using Rocket.Ollama.Domain.Project;
using Rocket.Page.Schemas.ProjectTaskTracker;

namespace Rocket.Ollama.Infrastructure.Project
{
    public class OllamaExtractProjectHook(IOllamaClient ollamaClient) : IIntegrationHook
    {
        private const string ExtractProjectPrompt = """
                                                    You are an OCR assistant. Extract all text exactly as written from this handwritten task tracker image, 
                                                    even if values appear unusual, incomplete, or ambiguous. 
                                                    Do not leave any field empty if there is any text visible in that cell. 
                                                    If a value is unclear, transcribe your best guess. 
                                                    Never return empty strings or null for cells that contain visible text.
                                                    """;
        
        /*
You are an OCR assistant. This image shows a handwritten project task tracker. 
The table has 4 columns: PROJECT, TASK, DUE (date), and EST. TIME (estimated time). 
Extract every row that has data written in it. Transcribe faithfully, including any 
unusual characters or unclear text. The NOTES section at the bottom contains free text.
         */

        public bool IsApplicable(BaseExecutionStep step) => step is OllamaExtractProjectExecutionStep;

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
                        .GetConnectorAsync<OllamaConnector>(
                            userId,
                            step,
                            cancellationToken
                        );

            var imageBytes =
                artifact
                    .Artifact;

            if (step is not OllamaExtractProjectExecutionStep ollamaStep)
                throw new RocketException(
                    "Unexpected step format, please check configuration",
                    ApiStatusCodeEnum.DeveloperError
                );

            var endpoint =
                connector.Endpoint ??
                throw new ConfigurationErrorsException(
                    nameof(connector.Endpoint)
                );

            var response =
                await
                    ollamaClient
                        .SendRequestAsync<ProjectTaskTrackerSchema>(
                            endpoint,
                            ollamaStep.ModelName,
                            ExtractProjectPrompt,
                            imageBytes,
                            RocketbookPageTemplateTypeEnum.ProjectTaskTracker,
                            useSchema: true,
                            temperature: 0.8F,
                            maxTokens: 2048, 
                            cancellationToken
                        );

            var responseJson =
                JsonSerializer
                    .Serialize(response);

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
    }
}