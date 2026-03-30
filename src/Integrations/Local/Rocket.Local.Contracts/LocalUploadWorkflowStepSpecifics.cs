using System.Text.Json.Serialization;
using Rocket.Api.Contracts.Workflows;

namespace Rocket.Local.Contracts
{
    public class LocalUploadWorkflowStepSpecifics : WorkflowStepSummary
    {
        [JsonPropertyName("upload_path")]
        public string UploadPath { get; set; }
    }
}