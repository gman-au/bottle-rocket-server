using System.Text.Json.Serialization;
using Rocket.Api.Contracts.Workflows;

namespace Rocket.OpenAi.Contracts.Project
{
    public class OpenAiExtractProjectWorkflowStepSpecifics : WorkflowStepSummary
    {
        [JsonPropertyName("first_pass_model_name")]
        public string FirstPassModelName { get; set; }
        
        [JsonPropertyName("second_pass_model_name")]
        public string SecondPassModelName { get; set; }
    }
}