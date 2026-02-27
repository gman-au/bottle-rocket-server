using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Rocket.Domain.Executions;
using Rocket.Domain.Jobs;
using Rocket.Gcp.Domain;
using Rocket.Integrations.Common;
using Rocket.Integrations.Common.Extensions;
using Rocket.Interfaces;

namespace Rocket.Gcp.Infrastructure
{
    public class GcpOcrExtractHook(
        IVisionOcrService visionOcrService,
        ILogger<GcpOcrExtractHook> logger
    )
        : HookWithConnectorBase<GcpExtractExecutionStep, GcpConnector>(logger), IIntegrationHook
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
                GetArtifactAsBytes();

            var credential =
                Connector
                    .Credential;

            var result =
                await
                    visionOcrService
                        .ExtractHandwrittenTextAsync(
                            imageBytes,
                            credential,
                            cancellationToken
                        );

            await
                appendLogMessageCallback(
                    step.Id,
                    result
                );

            return
                result
                    .AsCompletedRawTextArtifact();
        }
    }
}