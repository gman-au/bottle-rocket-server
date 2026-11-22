using System.Text.Json.Serialization;
using Rocket.Api.Contracts.Workflows;

namespace Rocket.OpenAi.Contracts.Retitle
{
    public class OpenAiRetitleFileWorkflowStepSpecifics : WorkflowStepSummary
    {
        [JsonPropertyName("model_name")]
        public string ModelName { get; set; }
        
        [JsonPropertyName("summarize_all_files")]
        public bool SummarizeAllFiles { get; set; }
    }
}