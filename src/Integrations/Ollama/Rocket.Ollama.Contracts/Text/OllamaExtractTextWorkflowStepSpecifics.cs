using System.Text.Json.Serialization;
using Rocket.Api.Contracts.Workflows;

namespace Rocket.Ollama.Contracts.Text
{
    public class OllamaExtractTextWorkflowStepSpecifics : WorkflowStepSummary
    {
        [JsonPropertyName("model_name")]
        public string ModelName { get; set; }
    }
}