using System.Text.Json.Serialization;

namespace Rocket.Replicate.Infrastructure.Definition
{
    public class ReplicateErrorResponse
    {
        [JsonPropertyName("detail")]
        public string Detail { get; set; }
    }
}