using System.Text.Json.Serialization;
using Rocket.Api.Contracts.Executions;

namespace Rocket.Ollama.Contracts.Project
{
    public class OllamaExtractProjectExecutionStepSpecifics : ExecutionStepSummary
    {
        [JsonPropertyName("first_pass_model_name")]
        public string FirstPassModelName { get; set; }
        
        [JsonPropertyName("second_pass_model_name")]
        public string SecondPassModelName { get; set; }
    }
}