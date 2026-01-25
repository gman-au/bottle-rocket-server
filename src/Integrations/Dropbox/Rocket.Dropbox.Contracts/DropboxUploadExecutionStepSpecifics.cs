using System.Text.Json.Serialization;
using Rocket.Api.Contracts.Executions;

namespace Rocket.Dropbox.Contracts
{
    public class DropboxUploadExecutionStepSpecifics : ExecutionStepSummary
    {
        [JsonPropertyName("subfolder")]
        public string Subfolder { get; set; }
    }
}