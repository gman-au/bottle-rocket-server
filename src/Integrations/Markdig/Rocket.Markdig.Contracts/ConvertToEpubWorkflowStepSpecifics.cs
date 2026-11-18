using System.Text.Json.Serialization;
using Rocket.Api.Contracts.Workflows;

namespace Rocket.Markdig.Contracts
{
    public class ConvertToEpubWorkflowStepSpecifics : WorkflowStepSummary
    {
        [JsonPropertyName("merge_files")]
        public bool MergeFiles { get; set; }
    }
}