using System.Text.Json.Serialization;
using Rocket.Api.Contracts.Workflows;

namespace Rocket.OpenAi.Contracts.WeeklyPlan
{
    public class OpenAiExtractWeeklyPlanWorkflowStepSpecifics : WorkflowStepSummary
    {
        [JsonPropertyName("model_name")]
        public string ModelName { get; set; }
    }
}