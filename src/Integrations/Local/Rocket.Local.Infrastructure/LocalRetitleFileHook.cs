using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Rocket.Domain.Enum;
using Rocket.Domain.Exceptions;
using Rocket.Domain.Executions;
using Rocket.Domain.Extensions;
using Rocket.Domain.Jobs;
using Rocket.Integrations.Common;
using Rocket.Integrations.Common.Extensions;
using Rocket.Interfaces;
using Rocket.Local.Domain;

namespace Rocket.Local.Infrastructure
{
    public class LocalRetitleFileHook(
        ILogger<LocalRetitleFileHook> logger
    )
        : HookBase<LocalRetitleFileExecutionStep>(logger), IIntegrationHook
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
                .InitializeArtifact(this);

            var fileResults = new List<(string, string)>();

            var elements =
                Artifact
                    .Elements;

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

                    var documentText =
                        element
                            .ArtifactAsText();

                    // Basic sanitized file name
                    var title = 
                        documentText
                            .ToSanitizedFileName();

                    await
                        jobControl
                            .SucceedStepAsync(
                                $"Title suggestion: \"{title}\"",
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
                    "There was an error re-titling the file",
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