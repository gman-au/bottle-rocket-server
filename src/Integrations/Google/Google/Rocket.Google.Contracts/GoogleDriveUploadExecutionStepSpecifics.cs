using System.Text.Json.Serialization;
using Rocket.Api.Contracts.Executions;

namespace Rocket.Google.Contracts
{
    public class GoogleDriveUploadExecutionStepSpecifics : ExecutionStepSummary
    {
        [JsonPropertyName("parent_folder_id")]
        public string ParentFolderId { get; set; }
    }
}