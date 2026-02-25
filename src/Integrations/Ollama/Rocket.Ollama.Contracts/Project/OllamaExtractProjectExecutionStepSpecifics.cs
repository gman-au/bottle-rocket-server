using System.Text.Json.Serialization;
using Rocket.Api.Contracts.Executions;

namespace Rocket.Ollama.Contracts.Project
{
    public class OllamaExtractProjectExecutionStepSpecifics : ExecutionStepSummary
    {
        [JsonPropertyName("model_name")]
        public string ModelName { get; set; }
    }
}