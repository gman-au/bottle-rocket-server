using System.Text.Json.Serialization;
using Rocket.Page.Schemas.WeeklyPlan;

namespace Rocket.Api.Contracts.Transcriptions
{
    public class WeeklyPlanTranscriptionSummary : TranscriptionSummary
    {
        [JsonPropertyName("weekly_plan")]
        public WeeklyPlanSchema WeeklyPlan { get; set; }
    }
}