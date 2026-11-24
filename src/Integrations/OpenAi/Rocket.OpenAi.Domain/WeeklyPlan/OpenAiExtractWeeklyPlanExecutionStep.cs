using Rocket.Domain.Executions;

namespace Rocket.OpenAi.Domain.WeeklyPlan
{
    public record OpenAiExtractWeeklyPlanExecutionStep : BaseExecutionStep
    {
        public string ModelName { get; set; }
    }
}