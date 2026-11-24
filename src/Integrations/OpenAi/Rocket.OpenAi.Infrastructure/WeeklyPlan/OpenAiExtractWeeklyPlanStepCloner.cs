using System;
using Rocket.Infrastructure.Mapping;
using Rocket.OpenAi.Domain.WeeklyPlan;

namespace Rocket.OpenAi.Infrastructure.WeeklyPlan
{
    public class OpenAiExtractWeeklyPlanStepCloner(IServiceProvider serviceProvider)
        : StepModelClonerBase<OpenAiExtractWeeklyPlanWorkflowStep, OpenAiExtractWeeklyPlanExecutionStep>(serviceProvider)
    {
        public override OpenAiExtractWeeklyPlanExecutionStep Clone(OpenAiExtractWeeklyPlanWorkflowStep value)
        {
            var result =
                base
                    .Clone(value);

            result.ModelName = value.ModelName;

            return result;
        }
    }
}