using System;
using Rocket.Infrastructure.Mapping;
using Rocket.Mailgun.Contracts;
using Rocket.Mailgun.Domain;

namespace Rocket.Mailgun.Infrastructure
{
    public class MailgunSendEmailExecutionStepMapper(IServiceProvider serviceProvider)
        : ExecutionStepModelMapperBase<MailgunSendEmailExecutionStep, MailgunSendEmailExecutionStepSpecifics>(serviceProvider)
    {
        public override MailgunSendEmailExecutionStep For(MailgunSendEmailExecutionStepSpecifics value)
        {
            var result =
                base
                    .For(value);

            result.RecipientAddress = value.RecipientAddress;

            return result;
        }

        public override MailgunSendEmailExecutionStepSpecifics From(MailgunSendEmailExecutionStep value)
        {
            var result =
                base
                    .From(value);

            result.RecipientAddress = value.RecipientAddress;

            return result;
        }
    }
}