using System.Text.Json.Serialization;
using Rocket.Api.Contracts.Workflows;

namespace Rocket.Gcp.Contracts
{
    public class GcpUploadWorkflowStepSpecifics : WorkflowStepSummary
    {
        [JsonPropertyName("parent_folder_id")]
        public string ParentFolderId { get; set; }
    }
}