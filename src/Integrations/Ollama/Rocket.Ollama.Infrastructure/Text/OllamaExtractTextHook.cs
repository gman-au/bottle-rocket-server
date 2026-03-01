using System;
using System.Configuration;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Rocket.Domain.Enum;
using Rocket.Domain.Executions;
using Rocket.Domain.Jobs;
using Rocket.Integrations.Common.Extensions;
using Rocket.Interfaces;
using Rocket.Ollama.Domain;
using Rocket.Ollama.Domain.Text;

namespace Rocket.Ollama.Infrastructure.Text
{
    public class OllamaExtractTextHook(
        IOllamaClient ollamaClient,
        ILogger<OllamaExtractTextHook> logger
    )
        : OllamaHookBase<OllamaExtractTextExecutionStep, OllamaConnector>(
            ollamaClient,
            logger
        ), IIntegrationHook
    {
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

            var endpoint =
                Connector.Endpoint ??
                throw new ConfigurationErrorsException(
                    nameof(Connector.Endpoint)
                );

            await
                EnsureModelsExistAsync(
                    endpoint,
                    cancellationToken,
                    ExecutionStep.ModelName
                );

            var response =
                await
                    OllamaClient
                        .SendRequestAsync<string>(
                            endpoint,
                            ExecutionStep.ModelName,
                            "Perform OCR on this image and return only the text.",
                            imageBytes,
                            RocketbookPageTemplateTypeEnum.StandardLined,
                            useSchema: false,
                            null,
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