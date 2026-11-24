using System.Text.Json.Serialization;
using Rocket.Page.Schemas.ProjectTaskTracker;

namespace Rocket.Api.Contracts.Transcriptions
{
    public class ProjectTranscriptionSummary : TranscriptionSummary
    {
        [JsonPropertyName("project")]
        public ProjectTaskTrackerSchema Project { get; set; }
    }
}