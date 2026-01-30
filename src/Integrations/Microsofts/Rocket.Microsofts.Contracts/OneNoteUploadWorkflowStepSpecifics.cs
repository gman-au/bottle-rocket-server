using System.Text.Json.Serialization;
using Rocket.Api.Contracts.Workflows;

namespace Rocket.Microsofts.Contracts
{
    public class OneNoteUploadWorkflowStepSpecifics : WorkflowStepSummary
    {
        [JsonPropertyName("section_id")]
        public string SectionId { get; set; }
    }
}