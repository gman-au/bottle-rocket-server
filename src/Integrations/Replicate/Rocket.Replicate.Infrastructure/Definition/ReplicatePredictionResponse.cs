using System.Text.Json.Serialization;

namespace Rocket.Replicate.Infrastructure.Definition
{
    public class ReplicatePredictionResponse<T>
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        
        [JsonPropertyName("model")]
        public string Model { get; set; }
        
        [JsonPropertyName("status")]
        public string Status { get; set; }
        
        [JsonPropertyName("output")]
        public T Output { get; set; }
    }
}