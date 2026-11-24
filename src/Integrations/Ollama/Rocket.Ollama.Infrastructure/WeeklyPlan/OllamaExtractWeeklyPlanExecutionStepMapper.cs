using System;
using Rocket.Infrastructure.Mapping;
using Rocket.Ollama.Contracts.WeeklyPlan;
using Rocket.Ollama.Domain.WeeklyPlan;

namespace Rocket.Ollama.Infrastructure.WeeklyPlan
{
    public class OllamaExtractWeeklyPlanExecutionStepMapper(IServiceProvider serviceProvider)
        : ExecutionStepModelMapperBase<OllamaExtractWeeklyPlanExecutionStep, OllamaExtractWeeklyPlanExecutionStepSpecifics>(serviceProvider)
    {
        public override OllamaExtractWeeklyPlanExecutionStep For(OllamaExtractWeeklyPlanExecutionStepSpecifics value)
        {
            var result =
                base
                    .For(value);

            result.FirstPassModelName = value.FirstPassModelName;
            result.SecondPassModelName = value.SecondPassModelName;

            return result;
        }

        public override OllamaExtractWeeklyPlanExecutionStepSpecifics From(OllamaExtractWeeklyPlanExecutionStep value)
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