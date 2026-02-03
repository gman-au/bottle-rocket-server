using System.Text.Json.Serialization;
using Rocket.Api.Contracts.Workflows;

namespace Rocket.Google.Contracts
{
    public class GoogleDriveUploadWorkflowStepSpecifics : WorkflowStepSummary
    {
        [JsonPropertyName("parent_folder_id")]
        public string ParentFolderId { get; set; }
    }
}