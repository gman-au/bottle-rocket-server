using System.Text.Json.Serialization;
using Rocket.Api.Contracts.Workflows;

namespace Rocket.Ollama.Contracts
{
    public class OllamaExtractWorkflowStepSpecifics : WorkflowStepSummary
    {
        [JsonPropertyName("model_name")]
        public string ModelName { get; set; }
    }
}