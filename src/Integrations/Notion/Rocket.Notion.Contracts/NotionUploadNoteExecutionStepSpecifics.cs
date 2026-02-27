using System.Text.Json.Serialization;
using Rocket.Api.Contracts.Executions;

namespace Rocket.Notion.Contracts
{
    public class NotionUploadNoteExecutionStepSpecifics : ExecutionStepSummary
    {
        [JsonPropertyName("parent_note_id")]
        public string ParentNoteId { get; set; }
    }
}