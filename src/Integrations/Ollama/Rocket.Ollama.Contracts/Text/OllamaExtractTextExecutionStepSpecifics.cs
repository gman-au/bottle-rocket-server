using System.Text.Json.Serialization;
using Rocket.Api.Contracts.Executions;

namespace Rocket.Ollama.Contracts.Text
{
    public class OllamaExtractTextExecutionStepSpecifics : ExecutionStepSummary
    {
        [JsonPropertyName("model_name")]
        public string ModelName { get; set; }
    }
}