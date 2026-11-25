using System.Collections.Generic;
using Rocket.Domain.Enum;

namespace Rocket.Domain.Transcriptions.Projects
{
    public record ProjectTranscription : BaseTranscription
    {
        public string Notes { get; set; }
        
        public IEnumerable<ProjectItem> Items { get; set; }
        
        public override int WorkflowFormatType => (int)WorkflowFormatTypeEnum.ProjectTaskTrackerData;
    }
}