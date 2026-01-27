using System.Text.Json.Serialization;
using Rocket.Api.Contracts.Executions;
using Rocket.Api.Contracts.Workflows;

namespace Rocket.Notion.Contracts
{
    public class NotionUploadExecutionStepSpecifics : ExecutionStepSummary
    {
        [JsonPropertyName("parent_note_id")]
        public string ParentNoteId { get; set; }
    }
}