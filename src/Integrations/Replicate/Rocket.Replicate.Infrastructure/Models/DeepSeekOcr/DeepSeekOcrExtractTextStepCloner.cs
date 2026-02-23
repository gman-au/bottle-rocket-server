using System;
using Rocket.Infrastructure.Mapping;
using Rocket.Replicate.Domain.Models.DeepSeekOcr;

namespace Rocket.Replicate.Infrastructure.Models.DeepSeekOcr
{
    public class DeepSeekOcrExtractTextStepCloner(IServiceProvider serviceProvider)
        : StepModelClonerBase<DeepSeekOcrExtractTextWorkflowStep, DeepSeekOcrExtractTextExecutionStep>(serviceProvider)
    {
        public override DeepSeekOcrExtractTextExecutionStep Clone(DeepSeekOcrExtractTextWorkflowStep value)
        {
            var result =
                base
                    .Clone(value);

            return result;
        }
    }
}