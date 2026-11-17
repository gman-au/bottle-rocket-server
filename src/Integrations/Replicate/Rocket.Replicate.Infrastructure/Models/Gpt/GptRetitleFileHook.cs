using System;
using System.Collections.Generic;
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
using Rocket.Replicate.Domain;
using Rocket.Replicate.Domain.Models.Gpt;

namespace Rocket.Replicate.Infrastructure.Models.Gpt
{
    public class GptRetitleFileHook(
        ILogger<GptRetitleFileHook> logger,
        IFileRetitler fileRetitler,
        IGlobalSettingsRepository globalSettingsRepository,
        IReplicateClient replicateClient
    ) : IIntegrationHook
    {
        private const string AdditionalInstructions =
            "Please respond only in plain text, no Markdown. Do not include any file extensions e.g. (.md, .txt) in your response.";

        public bool IsApplicable(
            BaseExecutionStep step
        ) => step is GptRetitleFileExecutionStep;

        public async Task<ExecutionStepArtifact> ProcessAsync(
            IWorkflowExecutionContext context,
            BaseExecutionStep step,
            string userId,
            IJobControl jobControl,
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

            var fileResults = new List<(string, string)>();

            var elements =
                artifact
                    .Elements;

            if (step is not GptRetitleFileExecutionStep retitleStep)
                throw new RocketException(
                    "Unexpected step format, please check configuration",
                    ApiStatusCodeEnum.DeveloperError
                );

            var apiToken =
                connector
                    .ApiToken;

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

            const string systemPrompt =
                AdditionalInstructions +
                CommonPromptConstants.SystemPromptRetitleFile;

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
                                    "Parsed text into GPT prompt",
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

                        var title =
                            await
                                GetSummarizedTextFromInputAsync(
                                    new GptSummarizeInput
                                    {
                                        Prompt = userPrompt,
                                        SystemPrompt = systemPrompt
                                    },
                                    apiToken,
                                    timeoutInMinutes,
                                    cancellationToken
                                );

                        await
                            jobControl
                                .SucceedStepAsync(
                                    $"GPT title suggestion: \"{title}\"",
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

                var title =
                    await
                        GetSummarizedTextFromInputAsync(
                            new GptSummarizeInput
                            {
                                Prompt = userPrompt,
                                SystemPrompt = systemPrompt
                            },
                            apiToken,
                            timeoutInMinutes,
                            cancellationToken
                        );

                await
                    jobControl
                        .SucceedStepAsync(
                            $"GPT title suggestion: \"{title}\""
                        );

                summaryFileName = title;
            }

            if (!allCompleted)
            {
                throw new RocketException(
                    "There was an error re-titling the file via Replicate",
                    ApiStatusCodeEnum.ThirdPartyServiceError
                );
            }

            if (retitleStep.SummarizeAllFiles)
            {
                await
                    fileRetitler
                        .RetitleAsync(
                            summaryFileName,
                            artifact.BatchId,
                            artifact.UserId,
                            cancellationToken
                        );

                return
                    artifact
                        .AsArtifactWithRenamedSummary(summaryFileName);
            }

            return
                artifact
                    .AsArtifactWithRenamedFiles(
                        fileResults
                    );
        }

        private async Task<string> GetSummarizedTextFromInputAsync(
            GptSummarizeInput input,
            string apiToken,
            int timeoutInMinutes,
            CancellationToken cancellationToken
        )
        {
            // create the prediction
            var predictionId =
                await
                    replicateClient
                        .CreatePredictionAsync(
                            apiToken,
                            null,
                            input,
                            cancellationToken,
                            "v1/models/openai/gpt-5.2/predictions"
                        );

            logger
                .LogInformation(
                    "Created prediction in Replicate: {predictionId}",
                    predictionId
                );

            var result =
                await
                    replicateClient
                        .WaitUntilPredictionCompletesAsync(
                            apiToken,
                            predictionId,
                            timeoutInMinutes,
                            cancellationToken
                        );

            return result;
        }
    }
}