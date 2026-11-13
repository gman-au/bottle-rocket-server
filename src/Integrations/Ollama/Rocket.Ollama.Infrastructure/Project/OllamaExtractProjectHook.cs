using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Rocket.Domain.Enum;
using Rocket.Domain.Exceptions;
using Rocket.Domain.Executions;
using Rocket.Domain.Jobs;
using Rocket.Domain.Utils;
using Rocket.Integrations.Common.Extensions;
using Rocket.Interfaces;
using Rocket.Ollama.Domain;
using Rocket.Ollama.Domain.Project;
using Rocket.Page.Schemas.ProjectTaskTracker;

namespace Rocket.Ollama.Infrastructure.Project
{
    public class OllamaExtractProjectHook(
        IOllamaClient ollamaClient,
        IGlobalSettingsRepository globalSettingsRepository,
        ILogger<OllamaExtractProjectHook> logger
    )
        : OllamaHookBase<OllamaExtractProjectExecutionStep, OllamaConnector>(
            ollamaClient,
            logger
        ), IIntegrationHook
    {
        private const string FirstPassPrompt = """
                                               Extract all text from this image. 
                                               For each row in the table, identify which column each value belongs to. 
                                               Return as plain text in the format: ROW n: PROJECT=x, TASK=x, DUE=x, EST_TIME=x. Also extract any NOTES.
                                               """;

        private const string SecondPassPrompt = """
                                                "Structure the following extracted text into JSON:\n\n{0}"
                                                """;

        public async Task<ExecutionStepArtifact> ProcessAsync(
            IWorkflowExecutionContext context,
            BaseExecutionStep step,
            string userId,
            IJobControl jobControl,
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

            var fileResults = new List<(string, ProjectTaskTrackerSchema)>();

            var elements =
                Artifact
                    .Elements;

            var endpoint =
                Connector.Endpoint ??
                throw new ConfigurationErrorsException(
                    nameof(Connector.Endpoint)
                );

            await
                EnsureModelsExistAsync(
                    endpoint,
                    cancellationToken,
                    ExecutionStep.FirstPassModelName,
                    ExecutionStep.SecondPassModelName
                );

            var globalSettings =
                await
                    globalSettingsRepository
                        .GetGlobalSettingsAsync(cancellationToken);

            var timeoutInMinutes =
                globalSettings?.DefaultModelTimeoutInMinutes ??
                DomainConstants.GlobalDefaultModelTimeoutInMinutes;

            var allCompleted = true;

            foreach (var element in elements)
            {
                try
                {
                    await
                        jobControl
                            .StartStepAsync(
                                element.ScanId
                            );
                    
                    var firstPassResponse =
                        await
                            OllamaClient
                                .SendImageRequestAsync<string>(
                                    endpoint,
                                    ExecutionStep.FirstPassModelName,
                                    FirstPassPrompt,
                                    element.Artifact,
                                    null,
                                    useSchema: false,
                                    temperature: 0.1F,
                                    maxTokens: 1024,
                                    numCtx: null,
                                    timeoutInMinutes,
                                    cancellationToken: cancellationToken
                                );

                    var secondPassResponse =
                        await
                            OllamaClient
                                .SendImageRequestAsync<ProjectTaskTrackerSchema>(
                                    endpoint,
                                    ExecutionStep.SecondPassModelName,
                                    string.Format(
                                        SecondPassPrompt,
                                        firstPassResponse
                                    ),
                                    imageBytes: null,
                                    typeof(ProjectTaskTrackerSchema),
                                    useSchema: true,
                                    temperature: 0.3F,
                                    maxTokens: 1024,
                                    numCtx: 4096,
                                    timeoutInMinutes,
                                    cancellationToken: cancellationToken
                                );

                    await
                        jobControl
                            .SucceedStepAsync(
                                $"Ollama response: \"{JsonSerializer.Serialize(secondPassResponse)}\"",
                                element.ScanId
                            );

                    fileResults
                        .Add((element.ScanId, secondPassResponse));
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
            }

            if (!allCompleted)
            {
                throw new RocketException(
                    "There was an error extracting the text via Ollama",
                    ApiStatusCodeEnum.ThirdPartyServiceError
                );
            }

            return
                fileResults
                    .ToArray()
                    .AsCompletedProjectTaskTrackerDataArtifact();
        }
    }
}