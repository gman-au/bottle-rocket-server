using System.Text.Json.Serialization;
using Rocket.Api.Contracts.Workflows;

namespace Rocket.Ollama.Contracts.Project
{
    public class OllamaExtractProjectWorkflowStepSpecifics : WorkflowStepSummary
    {
        [JsonPropertyName("model_name")]
        public string ModelName { get; set; }
    }
}