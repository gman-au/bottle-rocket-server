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
using Rocket.Integrations.Common.Extensions;
using Rocket.Interfaces;
using Rocket.Ollama.Domain;
using Rocket.Ollama.Domain.Retitle;
using Rocket.Page.Schemas.RetitleFile;

namespace Rocket.Ollama.Infrastructure.Retitle
{
    public class OllamaRetitleFileHook(
        IOllamaClient ollamaClient,
        IGlobalSettingsRepository globalSettingsRepository,
        ILogger<OllamaRetitleFileHook> logger
    )
        : OllamaHookBase<OllamaRetitleFileExecutionStep, OllamaConnector>(
            ollamaClient,
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

                    const string systemPrompt =
                        "You are an expert editor. Read the provided document and recommend a short, accurate title summarizing the contents.";

                    const string userPromptTemplate =
                        @"Here is the document:\n\n<document>\n{0}\n</document>\n\nRecommend a short file name title based on the text above.";

                    var documentText =
                        element
                            .ArtifactAsText();

                    var userPrompt =
                        string
                            .Format(
                                userPromptTemplate,
                                documentText
                            );

                    var response =
                        await
                            OllamaClient
                                .SendTextRequestAsync<RetitleFileSchema>(
                                    endpoint,
                                    ExecutionStep.ModelName,
                                    systemPrompt,
                                    userPrompt,
                                    typeof(RetitleFileSchema),
                                    useSchema: true,
                                    null,
                                    null,
                                    null,
                                    timeoutInMinutes,
                                    cancellationToken
                                );

                    var title =
                        response?.RecommendedTitle;

                    await
                        jobControl
                            .SucceedStepAsync(
                                $"Ollama title suggestion: \"{title}\"",
                                element.ScanId
                            );

                    fileResults
                        .Add((element.ScanId, title));
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
                    "There was an error re-titling the file via Ollama",
                    ApiStatusCodeEnum.ThirdPartyServiceError
                );
            }

            return
                Artifact
                    .AsArtifactWithRenamedFiles(
                        fileResults
                    );
        }
    }
}