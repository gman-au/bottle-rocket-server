using System.Text.Json.Serialization;

namespace Rocket.Api.Contracts.Executions
{
    public class StartExecutionResponse : ApiResponse
    {
        [JsonPropertyName("is_started")]
        public bool IsStarted { get; set; }
    }
}