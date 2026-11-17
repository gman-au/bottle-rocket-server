using System;
using Rocket.Infrastructure.Mapping;
using Rocket.Replicate.Domain.Models.Gpt;

namespace Rocket.Replicate.Infrastructure.Models.Gpt
{
    public class GptRetitleFileStepCloner(IServiceProvider serviceProvider)
        : StepModelClonerBase<GptRetitleFileWorkflowStep, GptRetitleFileExecutionStep>(serviceProvider)
    {
        public override GptRetitleFileExecutionStep Clone(GptRetitleFileWorkflowStep value)
        {
            var result =
                base
                    .Clone(value);

            return result;
        }
    }
}