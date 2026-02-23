using System.Text.Json.Serialization;

namespace Rocket.Replicate.Infrastructure.Definition
{
    public class CreatePredictionResponse<T> where T : IReplicateInput
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        
        [JsonPropertyName("input")]
        public T Input { get; set; }
    }
}