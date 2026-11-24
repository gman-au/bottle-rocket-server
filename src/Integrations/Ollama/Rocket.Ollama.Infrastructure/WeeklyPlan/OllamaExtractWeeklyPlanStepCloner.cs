using System;
using Rocket.Infrastructure.Mapping;
using Rocket.Ollama.Domain.WeeklyPlan;

namespace Rocket.Ollama.Infrastructure.WeeklyPlan
{
    public class OllamaExtractWeeklyPlanStepCloner(IServiceProvider serviceProvider)
        : StepModelClonerBase<OllamaExtractWeeklyPlanWorkflowStep, OllamaExtractWeeklyPlanExecutionStep>(serviceProvider)
    {
        public override OllamaExtractWeeklyPlanExecutionStep Clone(OllamaExtractWeeklyPlanWorkflowStep value)
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