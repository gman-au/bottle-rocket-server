using System;
using Rocket.Infrastructure.Mapping;
using Rocket.OpenAi.Contracts.WeeklyPlan;
using Rocket.OpenAi.Domain.WeeklyPlan;

namespace Rocket.OpenAi.Infrastructure.WeeklyPlan
{
    public class OpenAiExtractWeeklyPlanExecutionStepMapper(IServiceProvider serviceProvider)
        : ExecutionStepModelMapperBase<OpenAiExtractWeeklyPlanExecutionStep, OpenAiExtractWeeklyPlanExecutionStepSpecifics>(serviceProvider)
    {
        public override OpenAiExtractWeeklyPlanExecutionStep For(OpenAiExtractWeeklyPlanExecutionStepSpecifics value)
        {
            var result =
                base
                    .For(value);

            result.ModelName = value.ModelName;

            return result;
        }

        public override OpenAiExtractWeeklyPlanExecutionStepSpecifics From(OpenAiExtractWeeklyPlanExecutionStep value)
        {
            var result =
                base
                    .From(value);

            result.ModelName = value.ModelName;

            return result;
        }
    }
}