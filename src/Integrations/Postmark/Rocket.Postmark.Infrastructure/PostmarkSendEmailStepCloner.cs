using System;
using Rocket.Infrastructure.Mapping;
using Rocket.Postmark.Domain;

namespace Rocket.Postmark.Infrastructure
{
    public class PostmarkSendEmailStepCloner(IServiceProvider serviceProvider)
        : StepModelClonerBase<PostmarkSendEmailWorkflowStep, PostmarkSendEmailExecutionStep>(serviceProvider)
    {
        public override PostmarkSendEmailExecutionStep Clone(PostmarkSendEmailWorkflowStep value)
        {
            var result =
                base
                    .Clone(value);

            result.RecipientAddress = value.RecipientAddress;

            return result;
        }
    }
}