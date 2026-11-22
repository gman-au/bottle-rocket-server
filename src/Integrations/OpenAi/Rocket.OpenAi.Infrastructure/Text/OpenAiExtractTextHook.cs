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
using Rocket.OpenAi.Domain.Text;

namespace Rocket.OpenAi.Infrastructure.Text
{
    public class OpenAiExtractTextHook(
        IOpenAiClient openAiClient,
        IGlobalSettingsRepository globalSettingsRepository,
        ILogger<OpenAiExtractTextHook> logger
    )
        : OpenAiHookBase<OpenAiExtractTextExecutionStep, OpenAiConnector>(
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

            var apiKey =
                Connector.ApiKey ??
                throw new ConfigurationErrorsException(
                    nameof(Connector.ApiKey)
                );

            await
                EnsureModelsExistAsync(
                    apiKey,
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
                    
                    var response =
                        await
                            OpenAiClient
                                .SendImageRequestAsync<string>(
                                    apiKey,
                                    ExecutionStep.ModelName,
                                    CommonPromptConstants.SystemPromptExtractTextFromImage,
                                    CommonPromptConstants.UserPromptExtractTextFromImage,
                                    element.Artifact,
                                    typeof(string),
                                    useSchema: false,
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

                    fileResults
                        .Add((element.ScanId, response));
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
                    .AsCompletedRawTextArtifact();
        }
    }
}