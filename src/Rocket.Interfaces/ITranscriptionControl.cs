using System;
using System.Threading.Tasks;
using Rocket.Page.Schemas.ProjectTaskTracker;

namespace Rocket.Interfaces
{
    public interface ITranscriptionControl
    {
        public Func<string, string, Task> AddTextTranscriptionCallback { get; set; }
        
        public Func<string, ProjectTaskTrackerSchema, Task> AddProjectTranscriptionCallback { get; set; }
    }
}