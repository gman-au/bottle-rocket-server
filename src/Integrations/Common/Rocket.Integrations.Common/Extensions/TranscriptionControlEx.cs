using System.Threading.Tasks;
using Rocket.Interfaces;
using Rocket.Page.Schemas.ProjectTaskTracker;
using Rocket.Page.Schemas.WeeklyPlan;

namespace Rocket.Integrations.Common.Extensions
{
    public static class TranscriptionControlEx
    {
        public static async Task TranscribeTextAsync(
            this ITranscriptionControl transcriptionControl,
            string scanId,
            string text
        )
        {
            await
                transcriptionControl
                    .AddTextTranscriptionCallback(
                        scanId,
                        text
                    );
        }        
        
        public static async Task TranscribeProjectAsync(
            this ITranscriptionControl transcriptionControl,
            string scanId,
            ProjectTaskTrackerSchema project
        )
        {
            await
                transcriptionControl
                    .AddProjectTranscriptionCallback(
                        scanId,
                        project
                    );
        }       
        
        public static async Task TranscribeWeeklyPlanAsync(
            this ITranscriptionControl transcriptionControl,
            string scanId,
            WeeklyPlanSchema weeklyPlan
        )
        {
            await
                transcriptionControl
                    .AddWeeklyPlanTranscriptionCallback(
                        scanId,
                        weeklyPlan
                    );
        }
    }
}