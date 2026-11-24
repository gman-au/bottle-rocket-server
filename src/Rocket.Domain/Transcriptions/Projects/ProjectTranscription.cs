using System.Collections.Generic;

namespace Rocket.Domain.Transcriptions.Projects
{
    public record ProjectTranscription : BaseTranscription
    {
        public string Notes { get; set; }
        
        public IEnumerable<ProjectItem> Items { get; set; }
    }
}