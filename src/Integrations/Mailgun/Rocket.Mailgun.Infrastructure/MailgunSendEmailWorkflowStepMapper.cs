using System;
using Rocket.Infrastructure.Mapping;
using Rocket.Mailgun.Contracts;
using Rocket.Mailgun.Domain;

namespace Rocket.Mailgun.Infrastructure
{
    public class MailgunSendEmailWorkflowStepMapper(IServiceProvider serviceProvider)
        : WorkflowStepModelMapperBase<MailgunSendEmailWorkflowStep, MailgunSendEmailWorkflowStepSpecifics>(serviceProvider)
    {
        public override MailgunSendEmailWorkflowStep For(MailgunSendEmailWorkflowStepSpecifics value)
        {
            var result =
                base
                    .For(value);

            result.RecipientAddress = value.RecipientAddress;

            return result;
        }

        public override MailgunSendEmailWorkflowStepSpecifics From(MailgunSendEmailWorkflowStep value)
        {
            var result =
                base
                    .From(value);

            result.RecipientAddress = value.RecipientAddress;

            return result;
        }
    }
}