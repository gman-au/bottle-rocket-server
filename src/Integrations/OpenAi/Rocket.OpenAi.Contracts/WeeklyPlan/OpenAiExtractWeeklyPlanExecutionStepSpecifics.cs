using System.Text.Json.Serialization;
using Rocket.Api.Contracts.Executions;

namespace Rocket.OpenAi.Contracts.WeeklyPlan
{
    public class OpenAiExtractWeeklyPlanExecutionStepSpecifics : ExecutionStepSummary
    {
        [JsonPropertyName("model_name")]
        public string ModelName { get; set; }
    }
}