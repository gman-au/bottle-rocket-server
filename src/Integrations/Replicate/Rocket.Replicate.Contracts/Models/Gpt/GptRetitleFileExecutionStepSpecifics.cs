using System.Text.Json.Serialization;
using Rocket.Api.Contracts.Executions;

namespace Rocket.Replicate.Contracts.Models.Gpt
{
    public class GptRetitleFileExecutionStepSpecifics : ExecutionStepSummary
    {
        [JsonPropertyName("summarize_all_files")]
        public bool SummarizeAllFiles { get; set; }
    }
}