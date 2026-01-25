using System;
using Rocket.Infrastructure.Mapping;
using Rocket.MaxOcr.Domain;

namespace Rocket.MaxOcr.Infrastructure
{
    public class MaxOcrExtractStepCloner(IServiceProvider serviceProvider)
        : StepModelClonerBase<MaxOcrExtractWorkflowStep, MaxOcrExtractExecutionStep>(serviceProvider)
    {
        public override MaxOcrExtractExecutionStep Clone(MaxOcrExtractWorkflowStep value)
        {
            var result =
                base
                    .Clone(value);

            return result;
        }
    }
}