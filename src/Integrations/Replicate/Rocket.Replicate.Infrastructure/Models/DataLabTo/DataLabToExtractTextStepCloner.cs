using System;
using Rocket.Infrastructure.Mapping;
using Rocket.Replicate.Domain.Models.DataLabTo;

namespace Rocket.Replicate.Infrastructure.Models.DataLabTo
{
    public class DataLabToExtractTextStepCloner(IServiceProvider serviceProvider)
        : StepModelClonerBase<DataLabToExtractTextWorkflowStep, DataLabToExtractTextExecutionStep>(serviceProvider)
    {
        public override DataLabToExtractTextExecutionStep Clone(DataLabToExtractTextWorkflowStep value)
        {
            var result =
                base
                    .Clone(value);

            return result;
        }
    }
}