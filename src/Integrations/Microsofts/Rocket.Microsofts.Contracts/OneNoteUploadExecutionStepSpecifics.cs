using System.Text.Json.Serialization;
using Rocket.Api.Contracts.Executions;

namespace Rocket.Microsofts.Contracts
{
    public class OneNoteUploadExecutionStepSpecifics : ExecutionStepSummary
    {
        [JsonPropertyName("parent_note")]
        public string ParentNote { get; set; }
    }
}