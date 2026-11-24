using System;
using Rocket.Domain.Workflows;
using Rocket.Interfaces;
using Rocket.Ollama.Contracts.WeeklyPlan;

namespace Rocket.Ollama.Injection.Serialization
{
    public class OllamaExtractWeeklyPlanWorkflowDiscriminator : IJsonTypeDiscriminator<BaseWorkflowStep>
    {
        public Type Key => typeof(OllamaExtractWeeklyPlanWorkflowStepSpecifics);
        
        public string Value => "ollama_extract_weekly_plan_workflow";
    }
}