using System.Text.Json.Serialization;
using Rocket.Api.Contracts.Workflows;

namespace Rocket.Notion.Contracts
{
    public class NotionUploadWorkflowStepSpecifics : WorkflowStepSummary
    {
        [JsonPropertyName("parent_note_id")]
        public string ParentNoteId { get; set; }
    }
}