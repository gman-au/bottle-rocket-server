using System;
using Rocket.Infrastructure.Mapping;
using Rocket.Ollama.Contracts.WeeklyPlan;
using Rocket.Ollama.Domain.WeeklyPlan;

namespace Rocket.Ollama.Infrastructure.WeeklyPlan
{
    public class OllamaExtractWeeklyPlanWorkflowStepMapper(IServiceProvider serviceProvider)
        : WorkflowStepModelMapperBase<OllamaExtractWeeklyPlanWorkflowStep, OllamaExtractWeeklyPlanWorkflowStepSpecifics>(serviceProvider)
    {
        public override OllamaExtractWeeklyPlanWorkflowStep For(OllamaExtractWeeklyPlanWorkflowStepSpecifics value)
        {
            var result =
                base
                    .For(value);

            result.FirstPassModelName = value.FirstPassModelName;
            result.SecondPassModelName = value.SecondPassModelName;

            return result;
        }

        public override OllamaExtractWeeklyPlanWorkflowStepSpecifics From(OllamaExtractWeeklyPlanWorkflowStep value)
        {
            var result =
                base
                    .From(value);

            result.FirstPassModelName = value.FirstPassModelName;
            result.SecondPassModelName = value.SecondPassModelName;

            return result;
        }
    }
}