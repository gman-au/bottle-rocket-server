using System.Text.Json.Serialization;
using Rocket.Api.Contracts.Workflows;

namespace Rocket.Microsofts.Contracts
{
    public class OneDriveUploadWorkflowStepSpecifics : WorkflowStepSummary
    {
        [JsonPropertyName("subfolder")]
        public string Subfolder { get; set; }
    }
}