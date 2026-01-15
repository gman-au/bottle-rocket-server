using System.Text.Json.Serialization;
using Rocket.Api.Contracts.Workflows;

namespace Rocket.Dropbox.Contracts
{
    public class DropboxUploadStepDetail : WorkflowStepSummary
    {
        [JsonPropertyName("subfolder")]
        public string Subfolder { get; set; }
    }
}