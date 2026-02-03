using System.Text.Json.Serialization;
using Rocket.Api.Contracts.Executions;

namespace Rocket.Gcp.Contracts
{
    public class GcpUploadExecutionStepSpecifics : ExecutionStepSummary
    {
        [JsonPropertyName("parent_folder_id")]
        public string ParentFolderId { get; set; }
    }
}