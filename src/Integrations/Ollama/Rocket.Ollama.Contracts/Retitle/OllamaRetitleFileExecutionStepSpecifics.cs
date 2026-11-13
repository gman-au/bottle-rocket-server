using System.Text.Json.Serialization;
using Rocket.Api.Contracts.Executions;

namespace Rocket.Ollama.Contracts.Retitle
{
    public class OllamaRetitleFileExecutionStepSpecifics : ExecutionStepSummary
    {
        [JsonPropertyName("model_name")]
        public string ModelName { get; set; }
    }
}