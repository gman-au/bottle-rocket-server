using System;
using System.Collections.Generic;
using System.Configuration;
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
using Rocket.OpenAi.Domain.Retitle;
using Rocket.Page.Schemas.RetitleFile;

namespace Rocket.OpenAi.Infrastructure.Retitle
{
    public class OpenAiRetitleFileHook(
        IFileRetitler fileRetitler,
        IOpenAiClient openAiClient,
        IGlobalSettingsRepository globalSettingsRepository,
        ILogger<OpenAiRetitleFileHook> logger
    )
        : OpenAiHookBase<OpenAiRetitleFileExecutionStep, OpenAiConnector>(
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

            var fileResults = new List<(string, string)>();

            var elements =
                Artifact
                    .Elements;
            
            if (step is not OpenAiRetitleFileExecutionStep retitleStep)
                throw new RocketException(
                    "Unexpected step format, please check configuration",
                    ApiStatusCodeEnum.DeveloperError
                );

            var endpoint =
                Connector.ApiKey ??
                throw new ConfigurationErrorsException(
                    nameof(Connector.ApiKey)
                );

            await
                EnsureModelsExistAsync(
                    endpoint,
                    cancellationToken,
                    ExecutionStep.ModelName
                );

            var globalSettings =
                await
                    globalSettingsRepository
                        .GetGlobalSettingsAsync(cancellationToken);

            var timeoutInMinutes =
                globalSettings?.DefaultModelTimeoutInMinutes ??
                DomainConstants.GlobalDefaultModelTimeoutInMinutes;

            var allCompleted = true;

            var summaryPrompt = string.Empty;
            var summaryFileName = string.Empty;

            const string systemPrompt = CommonPromptConstants.SystemPromptRetitleFile;
            const string userPromptTemplate = CommonPromptConstants.UserPromptRetitleFileTemplate;

            foreach (var element in elements)
            {
                try
                {
                    await
                        jobControl
                            .StartStepAsync(
                                element.ScanId
                            );

                    var documentText =
                        element
                            .ArtifactAsText();
                    
                    if (retitleStep.SummarizeAllFiles)
                    {
                        summaryPrompt += Environment.NewLine + documentText;

                        await
                            jobControl
                                .SucceedStepAsync(
                                    "Parsed text into OpenAI prompt",
                                    element.ScanId
                                );
                    }
                    else
                    {
                        var userPrompt =
                            string
                                .Format(
                                    userPromptTemplate,
                                    documentText
                                );

                        var response =
                            await
                                openAiClient
                                    .SendTextRequestAsync<RetitleFileSchema>(
                                        endpoint,
                                        ExecutionStep.ModelName,
                                        systemPrompt,
                                        userPrompt,
                                        typeof(RetitleFileSchema),
                                        useSchema: true,
                                        temperature: 0,
                                        maxTokens: null,
                                        null,
                                        timeoutInMinutes,
                                        cancellationToken
                                    );

                        var title =
                            response?.RecommendedTitle;

                        await
                            jobControl
                                .SucceedStepAsync(
                                    $"OpenAI title suggestion: \"{title}\"",
                                    element.ScanId
                                );

                        fileResults
                            .Add((element.ScanId, title));
                    }
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
            
            if (retitleStep.SummarizeAllFiles)
            {
                var userPrompt =
                    string
                        .Format(
                            userPromptTemplate,
                            summaryPrompt
                        );

                var response =
                    await
                        openAiClient
                            .SendTextRequestAsync<RetitleFileSchema>(
                                endpoint,
                                ExecutionStep.ModelName,
                                systemPrompt,
                                userPrompt,
                                typeof(RetitleFileSchema),
                                useSchema: true,
                                temperature: 0,
                                maxTokens: null,
                                null,
                                timeoutInMinutes,
                                cancellationToken
                            );

                var title =
                    response?.RecommendedTitle;

                await
                    jobControl
                        .SucceedStepAsync(
                            $"OpenAI title suggestion: \"{title}\""
                        );

                summaryFileName = title;
            }

            if (!allCompleted)
            {
                throw new RocketException(
                    "There was an error re-titling the file via OpenAI",
                    ApiStatusCodeEnum.ThirdPartyServiceError
                );
            }

            if (retitleStep.SummarizeAllFiles)
            {
                await
                    fileRetitler
                        .RetitleAsync(
                            summaryFileName,
                            Artifact.BatchId,
                            Artifact.UserId,
                            cancellationToken
                        );

                return
                    Artifact
                        .AsArtifactWithRenamedSummary(summaryFileName);
            }

            return
                Artifact
                    .AsArtifactWithRenamedFiles(
                        fileResults
                    );
        }
    }
}