using System;
using System.Configuration;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rocket.Domain.Enum;
using Rocket.Domain.Exceptions;
using Rocket.Domain.Executions;
using Rocket.Domain.Jobs;
using Rocket.Interfaces;
using Rocket.Ollama.Domain;
using Rocket.Ollama.Domain.Text;

namespace Rocket.Ollama.Infrastructure.Text
{
    public class OllamaExtractTextHook(IOllamaClient ollamaClient) : IIntegrationHook
    {
        public bool IsApplicable(BaseExecutionStep step) => step is OllamaExtractTextExecutionStep;

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

            if (step is not OllamaExtractTextExecutionStep ollamaStep)
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
                        .SendRequestAsync<string>(
                            endpoint,
                            ollamaStep.ModelName,
                            "Perform OCR on this image and return only the text.",
                            imageBytes,
                            RocketbookPageTemplateTypeEnum.StandardLined,
                            useSchema: false,
                            null,
                            null,
                            cancellationToken
                        );

            await
                appendLogMessageCallback(
                    step.Id,
                    response
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
                                response
                            ),
                    FileExtension = ".txt"
                };

            return resultArtifact;
        }
    }
}