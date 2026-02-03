using System;
using Rocket.Infrastructure.Mapping;
using Rocket.QuestPdf.Domain;

namespace Rocket.QuestPdf.Infrastructure
{
    public class ConvertToPdfStepCloner(IServiceProvider serviceProvider)
        : StepModelClonerBase<ConvertToPdfWorkflowStep, ConvertToPdfExecutionStep>(serviceProvider)
    {
        public override ConvertToPdfExecutionStep Clone(ConvertToPdfWorkflowStep value)
        {
            var result =
                base
                    .Clone(value);

            return result;
        }
    }
}