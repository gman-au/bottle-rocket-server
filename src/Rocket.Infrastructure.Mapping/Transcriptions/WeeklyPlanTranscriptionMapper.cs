using Rocket.Api.Contracts.Transcriptions;
using Rocket.Domain.Transcriptions.WeeklyPlans;
using Rocket.Page.Schemas.WeeklyPlan;

namespace Rocket.Infrastructure.Mapping.Transcriptions
{
    public class WeeklyPlanTranscriptionMapper
        : TranscriptionModelMapperBase<WeeklyPlanTranscription, WeeklyPlanTranscriptionSummary>
    {
        public override WeeklyPlanTranscription For(
            WeeklyPlanTranscriptionSummary value
        )
        {
            var result =
                base
                    .For(value);

            result.Monday = value?.WeeklyPlan?.Monday;
            result.Tuesday = value?.WeeklyPlan?.Tuesday;
            result.Wednesday = value?.WeeklyPlan?.Wednesday;
            result.Thursday = value?.WeeklyPlan?.Thursday;
            result.Friday = value?.WeeklyPlan?.Friday;
            result.Saturday = value?.WeeklyPlan?.Saturday;
            result.Sunday = value?.WeeklyPlan?.Sunday;

            return result;
        }

        public override WeeklyPlanTranscriptionSummary From(
            WeeklyPlanTranscription value
        )
        {
            var result =
                base
                    .From(value);

            result.WeeklyPlan =
                new WeeklyPlanSchema
                {
                    Monday = value?.Monday,
                    Tuesday = value?.Tuesday,
                    Wednesday = value?.Wednesday,
                    Thursday = value?.Thursday,
                    Friday = value?.Friday,
                    Saturday = value?.Saturday,
                    Sunday = value?.Sunday,
                };

            return result;
        }
    }
}