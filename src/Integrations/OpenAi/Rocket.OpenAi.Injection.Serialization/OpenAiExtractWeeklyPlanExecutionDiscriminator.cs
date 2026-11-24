using System;
using Rocket.Domain.Executions;
using Rocket.Interfaces;
using Rocket.OpenAi.Contracts.WeeklyPlan;

namespace Rocket.OpenAi.Injection.Serialization
{
    public class OpenAiExtractWeeklyPlanExecutionDiscriminator : IJsonTypeDiscriminator<BaseExecutionStep>
    {
        public Type Key => typeof(OpenAiExtractWeeklyPlanExecutionStepSpecifics);
        
        public string Value => "openai_extract_weekly_plan_execution";
    }
}