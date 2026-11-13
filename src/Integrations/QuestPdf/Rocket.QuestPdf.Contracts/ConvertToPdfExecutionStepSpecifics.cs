using System.Text.Json.Serialization;
using Rocket.Api.Contracts.Executions;

namespace Rocket.QuestPdf.Contracts
{
    public class ConvertToPdfExecutionStepSpecifics : ExecutionStepSummary
    {
        [JsonPropertyName("merge_files")]
        public bool MergeFiles { get; set; }
    }
}