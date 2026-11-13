using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Rocket.Domain.Enum;
using Rocket.Domain.Exceptions;
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

            var elements = Artifact.Elements ?? [];

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

            var fileName = $"{element.FileName}{element.FileExtension}";
            var subjectName = $"Bottle Rocket attached note - {Artifact.FileName}";

            var sendResult =
                await
                    emailSender
                        .SendEmailAsync(
                            Connector.ServerToken,
                            element.Artifact,
                            fileName,
                            ExecutionStep.RecipientAddress,
                            Connector.SenderAddress,
                            subjectName,
                            cancellationToken
                        );

            await
                jobControl
                    .SucceedStepAsync(
                        $"Email sent from PostMark, response: {{{sendResult}}}",
                        element.ScanId
                    );
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
                    "There was an error sending the data via Mailgun",
                    ApiStatusCodeEnum.ThirdPartyServiceError
                );
            }

            return
                ExecutionStepArtifact
                    .Empty;
        }
    }
}