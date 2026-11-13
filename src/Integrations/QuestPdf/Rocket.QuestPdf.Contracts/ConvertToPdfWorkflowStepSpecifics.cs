using System.Text.Json.Serialization;
using Rocket.Api.Contracts.Workflows;

namespace Rocket.QuestPdf.Contracts
{
    public class ConvertToPdfWorkflowStepSpecifics : WorkflowStepSummary
    {
        [JsonPropertyName("merge_files")]
        public bool MergeFiles { get; set; }
    }
}