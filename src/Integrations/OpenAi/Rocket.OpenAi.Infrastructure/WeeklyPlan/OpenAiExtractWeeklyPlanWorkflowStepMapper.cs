using System;
using Rocket.Infrastructure.Mapping;
using Rocket.OpenAi.Contracts.WeeklyPlan;
using Rocket.OpenAi.Domain.WeeklyPlan;

namespace Rocket.OpenAi.Infrastructure.WeeklyPlan
{
    public class OpenAiExtractWeeklyPlanWorkflowStepMapper(IServiceProvider serviceProvider)
        : WorkflowStepModelMapperBase<OpenAiExtractWeeklyPlanWorkflowStep, OpenAiExtractWeeklyPlanWorkflowStepSpecifics>(serviceProvider)
    {
        public override OpenAiExtractWeeklyPlanWorkflowStep For(OpenAiExtractWeeklyPlanWorkflowStepSpecifics value)
        {
            var result =
                base
                    .For(value);

            result.ModelName = value.ModelName;

            return result;
        }

        public override OpenAiExtractWeeklyPlanWorkflowStepSpecifics From(OpenAiExtractWeeklyPlanWorkflowStep value)
        {
            var result =
                base
                    .From(value);

            result.ModelName = value.ModelName;

            return result;
        }
    }
}