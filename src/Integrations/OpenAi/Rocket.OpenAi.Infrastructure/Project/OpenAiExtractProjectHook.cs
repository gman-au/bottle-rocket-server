using System;
using System.Collections.Generic;
using System.Text.Json;
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
using Rocket.OpenAi.Domain;
using Rocket.OpenAi.Domain.Project;
using Rocket.Page.Schemas.ProjectTaskTracker;

namespace Rocket.OpenAi.Infrastructure.Project
{
    public class OpenAiExtractProjectHook(
        IGlobalSettingsRepository globalSettingsRepository,
        IOpenAiClient openAiClient,
        ILogger<OpenAiExtractProjectHook> logger
    ) : OpenAiHookBase<OpenAiExtractProjectExecutionStep, OpenAiConnector>(
        openAiClient,
        logger
    ), IIntegrationHook
    {
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

            var apiKey =
                Connector
                    .ApiKey;

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
                try
                {
                    await
                        jobControl
                            .StartStepAsync(
                                element.ScanId
                            );


                    var response =
                        await
                            OpenAiClient
                                .SendImageRequestAsync<ProjectTaskTrackerSchema>(
                                    apiKey,
                                    ExecutionStep.ModelName,
                                    CommonPromptConstants.SystemPromptExtractTextFromImage,
                                    CommonPromptConstants.UserPromptExtractTextFromImage,
                                    element.Artifact,
                                    typeof(ProjectTaskTrackerSchema),
                                    useSchema: true,
                                    temperature: 0,
                                    maxTokens: null,
                                    timeoutInMinutes,
                                    element.FileExtension,
                                    cancellationToken
                                );

                    await
                        jobControl
                            .SucceedStepAsync(
                                $"OpenAI response: \"{response}\"",
                                element.ScanId
                            );

                    if (response is not { } projectTaskTrackerSchema)
                        throw new RocketException(
                            $"There was an error parsing the data returned from the model to type {typeof(ProjectTaskTrackerSchema)}.",
                            ApiStatusCodeEnum.ThirdPartyServiceError
                        );

                    await
                        jobControl
                            .Transcription
                            .TranscribeProjectAsync(
                                element.ScanId,
                                response
                            );
                    
                    await
                        jobControl
                            .SucceedStepAsync(
                                $"OpenAI OCR response: \"{JsonSerializer
                                    .Serialize(projectTaskTrackerSchema)}\"",
                                element.ScanId
                            );

                    fileResults
                        .Add((element.ScanId, projectTaskTrackerSchema));
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
                    "There was an error extracting the text via OpenAI",
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