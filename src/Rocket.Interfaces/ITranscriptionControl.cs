using System;
using System.Threading.Tasks;
using Rocket.Page.Schemas.ProjectTaskTracker;
using Rocket.Page.Schemas.WeeklyPlan;

namespace Rocket.Interfaces
{
    public interface ITranscriptionControl
    {
        public Func<string, string, Task> AddTextTranscriptionCallback { get; set; }
        
        public Func<string, ProjectTaskTrackerSchema, Task> AddProjectTranscriptionCallback { get; set; }
        
        public Func<string, WeeklyPlanSchema, Task> AddWeeklyPlanTranscriptionCallback { get; set; }
    }
}