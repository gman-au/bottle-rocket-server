using System.Text.Json.Serialization;

namespace Rocket.Api.Contracts.Executions
{
    public class CancelExecutionResponse : ApiResponse
    {
        [JsonPropertyName("is_cancelled")]
        public bool IsCancelled { get; set; }
    }
}