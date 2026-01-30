using System.Text.Json.Serialization;
using Rocket.Api.Contracts.Executions;

namespace Rocket.Microsofts.Contracts
{
    public class OneDriveUploadExecutionStepSpecifics : ExecutionStepSummary
    {
        [JsonPropertyName("subfolder")]
        public string Subfolder { get; set; }
    }
}