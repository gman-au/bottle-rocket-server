using System.Text.Json.Serialization;

namespace Rocket.Api.Contracts.Batches
{
    public class CloseBatchResponse : ApiResponse
    {
        [JsonPropertyName("is_started")]
        public bool IsStarted { get; set; }
    }
}