using System.Text.Json.Serialization;
using Rocket.Api.Contracts.Workflows;

namespace Rocket.OpenAi.Contracts.Project
{
    public class OpenAiExtractProjectWorkflowStepSpecifics : WorkflowStepSummary
    {
        [JsonPropertyName("model_name")]
        public string ModelName { get; set; }
    }
}