using System.Text.Json.Serialization;
using Rocket.Api.Contracts.Workflows;

namespace Rocket.Ollama.Contracts.Project
{
    public class OllamaExtractProjectWorkflowStepSpecifics : WorkflowStepSummary
    {
        [JsonPropertyName("first_pass_model_name")]
        public string FirstPassModelName { get; set; }
        
        [JsonPropertyName("second_pass_model_name")]
        public string SecondPassModelName { get; set; }
    }
}