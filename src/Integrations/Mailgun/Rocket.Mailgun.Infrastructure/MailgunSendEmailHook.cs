using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Rocket.Domain.Executions;
using Rocket.Domain.Jobs;
using Rocket.Integrations.Common;
using Rocket.Integrations.Common.Extensions;
using Rocket.Interfaces;
using Rocket.Mailgun.Domain;

namespace Rocket.Mailgun.Infrastructure
{
    public class MailgunSendEmailHook(
        ILogger<MailgunSendEmailHook> logger,
        IMailgunEmailSender emailSender
    ) : HookWithConnectorBase<MailgunSendEmailExecutionStep, MailgunConnector>(logger), IIntegrationHook
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

            await
                emailSender
                    .SendEmailAsync(
                        Connector.SenderDomain,
                        Connector.ApiKey,
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
                    "Email sent from Mailgun"
                );

            return
                ExecutionStepArtifact
                    .Empty;
        }
    }
}