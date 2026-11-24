using System;
using Rocket.Domain.Executions;
using Rocket.Interfaces;
using Rocket.Ollama.Contracts.WeeklyPlan;

namespace Rocket.Ollama.Injection.Serialization
{
    public class OllamaExtractWeeklyPlanExecutionDiscriminator : IJsonTypeDiscriminator<BaseExecutionStep>
    {
        public Type Key => typeof(OllamaExtractWeeklyPlanExecutionStepSpecifics);
        
        public string Value => "ollama_extract_weekly_plan_execution";
    }
}