using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Rocket.Domain.Executions;
using Rocket.Domain.Jobs;
using Rocket.Integrations.Common;
using Rocket.Integrations.Common.Extensions;
using Rocket.Interfaces;
using Rocket.Postmark.Domain;

namespace Rocket.Postmark.Infrastructure
{
    public class PostmarkSendEmailHook(
        ILogger<PostmarkSendEmailHook> logger,
        IPostmarkEmailSender emailSender
    ) : HookWithConnectorBase<PostmarkSendEmailExecutionStep, PostmarkConnector>(logger), IIntegrationHook
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

            var fileName = $"{Artifact.FileName}{Artifact.FileExtension}";
            var subjectName = $"Bottle Rocket attached note - {Artifact.FileName}";

            var sendResult =
                await
                    emailSender
                        .SendEmailAsync(
                            Connector.ServerToken,
                            Artifact.Artifact,
                            fileName,
                            ExecutionStep.RecipientAddress,
                            Connector.SenderAddress,
                            subjectName,
                            cancellationToken
                        );

            await
                appendLogMessageCallback(
                    step.Id,
                    sendResult
                );

            return
                ExecutionStepArtifact
                    .Empty;
        }
    }
}