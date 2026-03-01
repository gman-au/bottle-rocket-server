using System;
using Rocket.Infrastructure.Mapping;
using Rocket.Replicate.Domain.Models.DataLabTo;

namespace Rocket.Replicate.Infrastructure.Models.DataLabTo.Project
{
    public class DataLabToExtractProjectStepCloner(IServiceProvider serviceProvider)
        : StepModelClonerBase<DataLabToExtractProjectWorkflowStep, DataLabToExtractProjectExecutionStep>(serviceProvider)
    {
        public override DataLabToExtractProjectExecutionStep Clone(DataLabToExtractProjectWorkflowStep value)
        {
            var result =
                base
                    .Clone(value);

            return result;
        }
    }
}