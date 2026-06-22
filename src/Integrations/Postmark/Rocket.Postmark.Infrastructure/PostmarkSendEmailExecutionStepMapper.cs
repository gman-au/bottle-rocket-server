using System;
using Rocket.Infrastructure.Mapping;
using Rocket.Postmark.Contracts;
using Rocket.Postmark.Domain;

namespace Rocket.Postmark.Infrastructure
{
    public class PostmarkSendEmailExecutionStepMapper(IServiceProvider serviceProvider)
        : ExecutionStepModelMapperBase<PostmarkSendEmailExecutionStep, PostmarkSendEmailExecutionStepSpecifics>(serviceProvider)
    {
        public override PostmarkSendEmailExecutionStep For(PostmarkSendEmailExecutionStepSpecifics value)
        {
            var result =
                base
                    .For(value);

            result.RecipientAddress = value.RecipientAddress;

            return result;
        }

        public override PostmarkSendEmailExecutionStepSpecifics From(PostmarkSendEmailExecutionStep value)
        {
            var result =
                base
                    .From(value);

            result.RecipientAddress = value.RecipientAddress;

            return result;
        }
    }
}