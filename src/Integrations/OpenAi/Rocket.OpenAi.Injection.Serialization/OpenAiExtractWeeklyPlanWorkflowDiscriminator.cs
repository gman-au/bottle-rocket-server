using System;
using Rocket.Domain.Workflows;
using Rocket.Interfaces;
using Rocket.OpenAi.Contracts.WeeklyPlan;

namespace Rocket.OpenAi.Injection.Serialization
{
    public class OpenAiExtractWeeklyPlanWorkflowDiscriminator : IJsonTypeDiscriminator<BaseWorkflowStep>
    {
        public Type Key => typeof(OpenAiExtractWeeklyPlanWorkflowStepSpecifics);
        
        public string Value => "openai_extract_weekly_plan_workflow";
    }
}