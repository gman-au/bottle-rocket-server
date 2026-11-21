using System;
using Rocket.Infrastructure.Mapping;
using Rocket.OpenAi.Domain.Project;

namespace Rocket.OpenAi.Infrastructure.Project
{
    public class OpenAiExtractProjectStepCloner(IServiceProvider serviceProvider)
        : StepModelClonerBase<OpenAiExtractProjectWorkflowStep, OpenAiExtractProjectExecutionStep>(serviceProvider)
    {
        public override OpenAiExtractProjectExecutionStep Clone(OpenAiExtractProjectWorkflowStep value)
        {
            var result =
                base
                    .Clone(value);

            result.FirstPassModelName = value.FirstPassModelName;
            result.SecondPassModelName = value.SecondPassModelName;

            return result;
        }
    }
}