using System;
using Rocket.Infrastructure.Mapping;
using Rocket.Mailgun.Domain;

namespace Rocket.Mailgun.Infrastructure
{
    public class MailgunSendEmailStepCloner(IServiceProvider serviceProvider)
        : StepModelClonerBase<MailgunSendEmailWorkflowStep, MailgunSendEmailExecutionStep>(serviceProvider)
    {
        public override MailgunSendEmailExecutionStep Clone(MailgunSendEmailWorkflowStep value)
        {
            var result =
                base
                    .Clone(value);

            result.RecipientAddress = value.RecipientAddress;

            return result;
        }
    }
}