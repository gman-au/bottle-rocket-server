using System.Text.Json.Serialization;
using Rocket.Api.Contracts.Executions;

namespace Rocket.OpenAi.Contracts.Retitle
{
    public class OpenAiRetitleFileExecutionStepSpecifics : ExecutionStepSummary
    {
        [JsonPropertyName("model_name")]
        public string ModelName { get; set; }
    }
}