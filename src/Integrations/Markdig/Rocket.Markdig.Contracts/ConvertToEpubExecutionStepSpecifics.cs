using System.Text.Json.Serialization;
using Rocket.Api.Contracts.Executions;

namespace Rocket.Markdig.Contracts
{
    public class ConvertToEpubExecutionStepSpecifics : ExecutionStepSummary
    {
        [JsonPropertyName("merge_files")]
        public bool MergeFiles { get; set; }
    }
}