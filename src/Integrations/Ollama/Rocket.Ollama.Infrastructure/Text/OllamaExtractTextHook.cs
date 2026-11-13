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
using Rocket.Ollama.Domain.Text;

namespace Rocket.Ollama.Infrastructure.Text
{
    public class OllamaExtractTextHook(
        IOllamaClient ollamaClient,
        IGlobalSettingsRepository globalSettingsRepository,
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
                    
                    var response =
                        await
                            OllamaClient
                                .SendImageRequestAsync<string>(
                                    endpoint,
                                    ExecutionStep.ModelName,
                                    "Perform OCR on this image and return only the text.",
                                    element.Artifact,
                                    typeof(string),
                                    useSchema: false,
                                    null,
                                    null,
                                    null,
                                    timeoutInMinutes,
                                    cancellationToken
                                );

                    await
                        jobControl
                            .SucceedStepAsync(
                                $"Ollama response: \"{response}\"",
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
                    "There was an error extracting the text via Ollama",
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