using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Rocket.Page.Schemas.ProjectTaskTracker
{
    public class ProjectTaskTrackerSchema
    {
        [JsonPropertyName("notes")]
        public string Notes { get; set; }
        
        [JsonPropertyName("items")]
        public IEnumerable<ProjectTaskTrackerItem> Items { get; set; }
    }
}