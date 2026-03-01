using System.Text.Json.Serialization;

namespace Rocket.Page.Schemas.ProjectTaskTracker
{
    public class ProjectTaskTrackerItem
    {
        [JsonPropertyName("project")]
        public string Project { get; set; }
        
        [JsonPropertyName("task")]
        public string Task { get; set; }
        
        [JsonPropertyName("due_date")]
        public string DueDate { get; set; }
        
        [JsonPropertyName("est_time")]
        public string EstimatedTime { get; set; }
    }
}