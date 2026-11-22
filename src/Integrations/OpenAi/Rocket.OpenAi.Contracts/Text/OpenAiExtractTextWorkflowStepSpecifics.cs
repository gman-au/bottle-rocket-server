using System.Text.Json.Serialization;
using Rocket.Api.Contracts.Workflows;

namespace Rocket.OpenAi.Contracts.Text
{
    public class OpenAiExtractTextWorkflowStepSpecifics : WorkflowStepSummary
    {
        [JsonPropertyName("model_name")]
        public string ModelName { get; set; }
    }
}