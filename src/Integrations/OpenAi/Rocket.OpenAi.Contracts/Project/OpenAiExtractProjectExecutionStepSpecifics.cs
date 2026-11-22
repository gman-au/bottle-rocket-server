using System.Text.Json.Serialization;
using Rocket.Api.Contracts.Executions;

namespace Rocket.OpenAi.Contracts.Project
{
    public class OpenAiExtractProjectExecutionStepSpecifics : ExecutionStepSummary
    {
        [JsonPropertyName("model_name")]
        public string ModelName { get; set; }
    }
}