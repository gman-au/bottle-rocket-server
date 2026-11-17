using System.Text.Json.Serialization;
using Rocket.Api.Contracts.Workflows;

namespace Rocket.Replicate.Contracts.Models.Gpt
{
    public class GptRetitleFileWorkflowStepSpecifics : WorkflowStepSummary
    {
        [JsonPropertyName("summarize_all_files")]
        public bool SummarizeAllFiles { get; set; }
    }
}