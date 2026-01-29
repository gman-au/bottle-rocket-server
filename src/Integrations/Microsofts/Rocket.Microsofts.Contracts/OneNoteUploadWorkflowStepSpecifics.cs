using System.Text.Json.Serialization;
using Rocket.Api.Contracts.Workflows;

namespace Rocket.Microsofts.Contracts
{
    public class OneNoteUploadWorkflowStepSpecifics : WorkflowStepSummary
    {
        [JsonPropertyName("parent_note")]
        public string ParentNote { get; set; }
    }
}