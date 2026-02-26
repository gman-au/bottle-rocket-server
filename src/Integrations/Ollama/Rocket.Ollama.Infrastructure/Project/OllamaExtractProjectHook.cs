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
        private const string FirstPassPrompt = """
                                               Extract all text from this image. 
                                               For each row in the table, identify which column each value belongs to. 
                                               Return as plain text in the format: ROW n: PROJECT=x, TASK=x, DUE=x, EST_TIME=x. Also extract any NOTES.
                                               """;
        
        private const string SecondPassPrompt = """
                                               "Structure the following extracted text into JSON:\n\n{0}"
                                               """;        


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

            var firstPassResponse =
                await
                    ollamaClient
                        .SendRequestAsync<string>(
                            endpoint,
                            ollamaStep.FirstPassModelName,
                            FirstPassPrompt,
                            imageBytes,
                            RocketbookPageTemplateTypeEnum.NotSet,
                            useSchema: false,
                            temperature: 0.1F,
                            maxTokens: 1024,
                            numCtx: null,
                            cancellationToken
                        );

            var secondPassResponse =
                await
                    ollamaClient
                        .SendRequestAsync<ProjectTaskTrackerSchema>(
                            endpoint,
                            ollamaStep.SecondPassModelName,
                            string.Format(SecondPassPrompt, firstPassResponse),
                            imageBytes: null,
                            RocketbookPageTemplateTypeEnum.ProjectTaskTracker,
                            useSchema: true,
                            temperature: 0.3F,
                            maxTokens: 1024,
                            numCtx: 4096,
                            cancellationToken
                        );

            var responseJson =
                JsonSerializer
                    .Serialize(secondPassResponse);

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