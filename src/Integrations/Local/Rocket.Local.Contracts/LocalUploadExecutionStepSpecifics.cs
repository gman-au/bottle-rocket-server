using System.Text.Json.Serialization;
using Rocket.Api.Contracts.Executions;

namespace Rocket.Local.Contracts
{
    public class LocalUploadExecutionStepSpecifics : ExecutionStepSummary
    {
        [JsonPropertyName("upload_path")]
        public string UploadPath { get; set; }
    }
}