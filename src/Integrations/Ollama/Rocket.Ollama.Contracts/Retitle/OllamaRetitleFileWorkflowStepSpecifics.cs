using System.Text.Json.Serialization;
using Rocket.Api.Contracts.Workflows;

namespace Rocket.Ollama.Contracts.Retitle
{
    public class OllamaRetitleFileWorkflowStepSpecifics : WorkflowStepSummary
    {
        [JsonPropertyName("model_name")]
        public string ModelName { get; set; }
    }
}